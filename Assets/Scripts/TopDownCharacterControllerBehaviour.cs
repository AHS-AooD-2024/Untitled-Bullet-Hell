using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// A character controller <see cref="MonoBehaviour"/> that uses a <see cref="Rigidbody2D"/>
/// to move a game object.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class TopDownCharacterControllerBehaviour : LookingGlass, ITopDownCharacterController {

    private Rigidbody2D m_rigidbody;

    public bool IsDashing { get => m_isDashing; }

    private bool m_isDashing = false;

    public Vector2 Velocity { get => m_velocity; }

    private Vector2 m_velocity = Vector2.zero;
    private Vector2 m_targetVelocity = Vector2.zero;
    private Vector2 m_velocityVelocity = Vector2.zero;

    public Vector2 Movement { get => m_move; }

    private Vector2 m_move = Vector2.zero;

    [Header("Movement")]
    [Space]

    [SerializeField]
    [Range(0.0F, 1.0F)]
    private float m_movementSmoothing = 0.05F;

    [SerializeField]
    [Range(1.0F, 10.0F)]
    private float m_movementSpeed = 4.0F;

    public float MovementSpeed { get => m_movementSpeed; }

    [Header("Dashing")]
    [Space]

    [SerializeField]
    private float m_dashStartSpeedCoefficient = 8.0F;

    [SerializeField]
    private float m_dashEndSpeedCoefficient = 8.0F;
    public float DashSpeedCoefficient(float t) {
        return Mathf.Lerp(m_dashStartSpeedCoefficient, m_dashEndSpeedCoefficient, t);
    }

    [SerializeField]
    private float m_dashDurationSeconds = 0.5F;

    public float DashDurationSeconds { get => m_dashDurationSeconds; }

    [SerializeField]
    [Range(0.0F, 1.0F)]
    private float m_dashCooldownSeconds = 0.33F;
    public float DashCooldownSeconds { get => m_dashCooldownSeconds; }

    /// <summary>
    /// Whether the character's movement direction can change during a dash.
    /// Disable for dodge-roll like dashing.
    /// </summary>
    [SerializeField]
    private bool m_canDashControl = true;
    public bool CanDashControl { get => m_canDashControl; }

    private float m_dashTime = 0.0F;
    private float m_dashCooldownTime = 0.0F;

    [Header("Animation")]
    [Space]

    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private bool m_doRotateTransform = true;

    /// <summary>
    /// The direction the character faces idly. This is used for automatic rotation.
    /// </summary>
    [SerializeField]
    private Vector2 m_idleFace = Vector2.up;

    private Vector2 m_lookDirection = Vector2.zero;
    private float m_lookAngle = 0.0F;

    public Vector2 LookDirection { get => m_lookDirection; }

    public override Vector2 Direction => LookDirection;

    public float LookAngle { get => m_lookAngle; }

    public Vector3 LookRotation { get => Vector3.forward * LookAngle; }

    #if UNITY_EDITOR
    [Header("Debug")]
    [Space]

    [SerializeField]
    private TextMeshProUGUI m_debugText;
    #endif

    protected virtual void Awake() {
        m_idleFace.Normalize();
        if(m_idleFace == Vector2.zero) {
            Debug.LogError("Cannot face a zero direction.");
        }

        if(m_animator == null) {
            m_animator = GetComponent<Animator>();
        }
        
        if(m_rigidbody == null) {
            m_rigidbody = GetComponent<Rigidbody2D>();
            
            // Gravity is in the y direction, which we kinda need
            m_rigidbody.gravityScale = 0.0F;
            m_rigidbody.freezeRotation = true;
        }
    }

    public void Move(Vector2 movement, Vector2 look, bool doDash, float deltaTime) {
        if (doDash && !m_isDashing) {
            m_isDashing = true;
            BroadcastMessage("OnDashStart", SendMessageOptions.DontRequireReceiver);
        }

        // disallow changing direction while dashing, unless we have dash control
        if(m_canDashControl || !m_isDashing) {
            m_move = movement.normalized;
        }
        m_targetVelocity = m_move * m_movementSpeed;

        look.Normalize();

        if(m_isDashing && m_dashCooldownTime <= 0.0F) {
            if(m_dashTime < m_dashDurationSeconds) {
                m_dashTime += deltaTime;
                m_targetVelocity *= DashSpeedCoefficient(m_dashTime / m_dashDurationSeconds);
                BroadcastMessage("OnDash", SendMessageOptions.DontRequireReceiver);
            } else {
                m_dashTime = 0.0F;
                m_dashCooldownTime = m_dashCooldownSeconds;
                m_isDashing = false;
                BroadcastMessage("OnDashEnd", SendMessageOptions.DontRequireReceiver);
            }
        }

        if (m_dashCooldownTime > 0.0F) {
            m_dashCooldownTime -= deltaTime;
        }

        m_velocity = Vector2.SmoothDamp(
            m_velocity, 
            m_targetVelocity, 
            ref m_velocityVelocity,
            m_movementSmoothing,
            Mathf.Infinity, 
            deltaTime
        );

        Vector2 translation = m_velocity;
        m_rigidbody.linearVelocity = translation;

        // this will be used later for animations
        if(look != Vector2.zero) {
            // Rotate the object to face in the direction of look
            m_lookDirection = look;
        } else if(m_move != Vector2.zero) {
            // Rotate the object to face the direction it is moving in.
            m_lookDirection = m_move;
        }
        m_lookAngle = Vector2.SignedAngle(m_idleFace, m_lookDirection);

        if(m_doRotateTransform) {
            transform.localEulerAngles = LookRotation;
        }

        // FaceTransform();

        // m_animator.SetFloat("Velocity X", m_velocity.x);
        // m_animator.SetFloat("Velocity Y", m_velocity.y);
        // m_animator.SetBool("Is Dashing", m_isDashing);

        StringBuilder sb = new(16);

        if(m_velocity.sqrMagnitude > 0.01F) {
            sb.Append("Move");
        } else {
            sb.Append("Idle");
        }

        // note m_lookAngle is in degrees along the domain [-180, 180]
        if(m_lookAngle > -67.5F && m_lookAngle < 67.5F) {
            sb.Append(" Up");
        } else if(m_lookAngle > 112.5F || m_lookAngle < -112.5F) {
            sb.Append(" Down");
        }

        if (m_lookAngle > 22.5F && m_lookAngle < 157.5) {
            sb.Append(" Left");
        } else if(m_lookAngle < -22.5F && m_lookAngle > -157.5F) {
            sb.Append(" Right");
        }

        string state = sb.ToString();        

        #if UNITY_EDITOR
        if(m_debugText != null)
            m_debugText.text = state + "\n" + m_lookAngle;
        #endif

        // TODO: changes between animations are a bit abrupt, and I know we can fix it by
        // starting new animations on the same frame index that the previous ended on. This
        // would line up the passive breathing, etc.
        // The problem is that the animator stuff doesn't really do what I am looking for.
        // I tried something, but it just made the animations freeze for a while when they change.

        // Exit time and stuff would also probably work
        
        // var stateInfo = m_animator.GetCurrentAnimatorStateInfo(-1);
        // float t = (stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime)) * stateInfo.length;
        // m_animator.PlayInFixedTime(state, -1, t);
        
        m_animator.PlayInFixedTime(state);
    }

    /// <summary>
    /// Causes the transform's scale to be negative when looking left and vice versa.
    /// </summary>
    private void FaceTransform() {
        Vector3 scale = transform.localScale;
        bool lookLeft = m_lookAngle > 0.0F;
        bool lookingLeft = scale.x < 0.0F;
        if(lookLeft != lookingLeft) {
            scale.x *= -1.0F;
            transform.localScale = scale;
        }
    }
}

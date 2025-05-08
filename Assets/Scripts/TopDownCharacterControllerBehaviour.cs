using System.Collections;
using System.Collections.Generic;
using System.Text;
using Nevelson.Topdown2DPitfall.Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

/// <summary>
/// A character controller <see cref="MonoBehaviour"/> that uses a <see cref="Rigidbody2D"/>
/// to move a game object.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class TopDownCharacterControllerBehaviour : LookingGlass, ITopDownCharacterController, IPitfallObject {

    private Rigidbody2D m_rigidbody;

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

    [Header("Animation")]
    [Space]

    [SerializeField]
    private bool m_doRotateTransform = true;

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

    private bool m_frozen;

    protected virtual void Awake() {
        
        if(m_rigidbody == null) {
            m_rigidbody = GetComponent<Rigidbody2D>();
            
            // Gravity is in the y direction, which we kinda need
            m_rigidbody.gravityScale = 0.0F;
            m_rigidbody.freezeRotation = true;
        }
    }

    public void Move(Vector2 movement, Vector2 look, float deltaTime) => Move(movement, look, false, deltaTime);

    public void Move(Vector2 movement, Vector2 look, bool doDash, float deltaTime) {
        if(m_frozen) {
            m_rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        m_targetVelocity = m_move * m_movementSpeed;

        look.Normalize();

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
        m_lookAngle = Vector2.SignedAngle(Vector2.up, m_lookDirection);

        if(m_doRotateTransform) {
            transform.localEulerAngles = LookRotation;
        }

        // FaceTransform();

        // m_animator.SetFloat("Velocity X", m_velocity.x);
        // m_animator.SetFloat("Velocity Y", m_velocity.y);
        // m_animator.SetBool("Is Dashing", m_isDashing);

        #if UNITY_EDITOR
        if(m_debugText != null)
            m_debugText.text = "\n" + m_lookAngle;
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
    }

    public void Stop() {
        m_rigidbody.linearVelocity = Vector2.zero;
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

    public void PitfallActionsBefore() {
        m_frozen = true;
    }

    public void PitfallResultingAfter() {
        m_frozen = false;
    }
}

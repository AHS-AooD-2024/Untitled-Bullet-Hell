using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A character controller <see cref="MonoBehaviour"/> that uses a <see cref="Rigidbody2D"/>
/// to move a game object.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class TopDownCharacterControllerBehaviour : MonoBehaviour, ITopDownCharacterController {

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

    [SerializeField]
    private bool m_doAutoDash = true;
    public bool DoAutoDash { get => m_doAutoDash; }

    private float m_dashTime = 0.0F;
    private float m_dashCooldownTime = 0.0F;

    [Header("Animation")]
    [Space]

    [SerializeField]
    private Animator m_animator;

    /// <summary>
    /// The direction the character faces idly. This is used for automatic rotation.
    /// </summary>
    [SerializeField]
    private Vector2 m_idleFace = Vector2.up;

    protected virtual void Awake() {
        m_idleFace.Normalize();
        if(m_idleFace == Vector2.zero) {
            Debug.LogError("Cannot face a zero direction.");
        }

        if(m_animator is null) {
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
        m_move = movement.normalized;
        m_targetVelocity = m_move * m_movementSpeed;
        m_isDashing |= doDash;

        look.Normalize();

        if(m_isDashing && m_dashCooldownTime <= 0.0F) {
            if(m_dashTime < m_dashDurationSeconds) {
                m_dashTime += deltaTime;
                m_targetVelocity *= DashSpeedCoefficient(m_dashTime / m_dashDurationSeconds);
            } else {
                m_dashTime = 0.0F;
                m_dashCooldownTime = m_dashCooldownSeconds;
                m_isDashing = false;
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

        if(look != Vector2.zero) {
            // Rotate the object to face in the direction of look
            float theta = Vector2.SignedAngle(m_idleFace, look);
            transform.localEulerAngles = Vector3.forward * theta;
        } else if(m_move != Vector2.zero) {
            // Rotate the object to face the direction it is moving in.
            float theta = Vector2.SignedAngle(m_idleFace, m_move);
            transform.localEulerAngles = Vector3.forward * theta;
        }

        m_animator.SetFloat("Velocity X", m_velocity.x);
        m_animator.SetFloat("Velocity Y", m_velocity.y);
        m_animator.SetBool("Is Dashing", m_isDashing);
    }
}

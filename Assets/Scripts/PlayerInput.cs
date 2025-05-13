using System.Text;
using Combat;
using UnityEngine;

[RequireComponent(typeof(ITopDownDashCharacterController), typeof(AttackBehaviour))]
public sealed class PlayerInput : MonoBehaviour, ILookingGlass {
    [SerializeField]
    private Animator m_movementAnimator;

    [SerializeField]
    private Animator m_shootingAnimator;

    private ITopDownDashCharacterController m_controller;
    private AttackBehaviour m_attacker;

    private Vector2 m_look;
    private Vector2 m_input;

    private bool m_doDash;

    [Header("Attacks")]
    [Space]

    // [SerializeField]
    // private bool m_doAutoSwing;
    // private bool m_doSwing;

    [SerializeField]
    private Transform m_shootFrom;

    [SerializeField]
    private Projectile2D m_projectile;

    [SerializeField]
    private bool m_doAutoFire;
    private bool m_doShoot;

    public Vector2 Direction => m_controller.Direction;

    public float Angle => m_controller.Angle;

    public float SignedAngle => m_controller.SignedAngle;

    public Vector3 EulerAngles => m_controller.EulerAngles;

    private bool m_isFacingRight;

    private void Awake() {
        m_controller = GetComponent<ITopDownDashCharacterController>();
        m_attacker = GetComponent<AttackBehaviour>();
    }

    private void Update() {
        Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_look = mouseWorldSpace - transform.position;

        m_input.x = Input.GetAxisRaw("Horizontal");
        m_input.y = Input.GetAxisRaw("Vertical");
        if(m_input != Vector2.zero && Input.GetButtonDown("Jump")) {
            m_doDash = true;
        }

        // if(m_doAutoSwing && Input.GetButton("Fire1") || Input.GetButtonDown("Fire1")) {
        //     m_doSwing = true;
        // }
        
        if(!m_doDash && (m_doAutoFire && Input.GetButton("Fire2") || Input.GetButtonDown("Fire2"))) {
            m_doShoot = true;
        }
    }

    private void FixedUpdate() {
        m_controller.Move(m_input, m_look, m_doDash, Time.fixedDeltaTime);
        m_doDash = false;

        StringBuilder sb = new(16);
        if(m_controller.Velocity.sqrMagnitude > 0.1F) {
            sb.Append("Move");
        } else {
            sb.Append("Idle");
        }

        if(SignedAngle < 90.0F && SignedAngle > -90.0F) {
            sb.Append(" Up");
        } else {
            sb.Append(" Down");
        }

        m_movementAnimator.PlayInFixedTime(sb.ToString());
        
        bool shouldFaceRight = SignedAngle > 0.0F;
        if(m_isFacingRight != shouldFaceRight) {
            Flip();
        }
        
        if(!m_controller.IsDashing){
            m_attacker.UpdateCooldowns(Time.fixedDeltaTime);

            if(m_doShoot) {
                m_attacker.Shoot(m_projectile, Direction, m_shootFrom.position - transform.position);
                m_doShoot = false;
                if(m_shootingAnimator != null)
                    m_shootingAnimator.PlayInFixedTime("Shoot");
            }
        }
    }

    private void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1.0F;
        transform.localScale = scale;
    }
}
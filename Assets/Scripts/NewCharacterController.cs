using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class NewCharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    private float xDir, yDir;
    private Vector2 direction;
    private Vector2 m_look = Vector2.zero;

    [Header("Dash")]
    [SerializeField] public KeyCode dashKey;
    [SerializeField] private float DashSpeed;
    [SerializeField] private float DashCooldown;
    [SerializeField] private float DashTime;
    private bool canDash = true;
    [HideInInspector] public bool isDashing = false;


    void Start()
    {
        //animator = transform.GetChild(0).GetComponent<Animator>();
        rb = transform.GetComponent<Rigidbody2D>();
        direction = new Vector2(0, 0);
    }
    void Update()
    {
        GetMoveDirection();
        //Flip();
        ChangeState();
        CheckDash();
        GetLookDirection();
    }

    void FixedUpdate()
    {
        Move();
        Dash();
    }

    void GetLookDirection() {
        Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_look = mouseWorldSpace - transform.position;
    }

    void Flip() {
        if (Input.GetKeyDown(KeyCode.D))
        transform.localScale = new Vector2(1, 1);
        if (Input.GetKeyDown(KeyCode.A))
        transform.localScale = new Vector2(-1, 1); 
    }

    void GetMoveDirection() {
        if (!isDashing) {
            xDir = Input.GetAxisRaw("Horizontal");
            yDir = Input.GetAxisRaw("Vertical");
        }  
    }

    void ChangeState() {
        if (xDir > 0) {
            animator.Play("Right");
        }
        if (xDir < 0) {
            animator.Play("Left");
        }
    }

    public void Move()
    {
        if (!isDashing) {
            direction.x = xDir;
            direction.y = yDir;
            direction = direction.normalized;
            rb.AddForce(direction * moveSpeed, ForceMode2D.Force);
        }  
    }

    private void CheckDash() {
        if (Input.GetKeyDown(dashKey) && canDash && (Math.Abs(xDir) > 0 || Math.Abs(yDir) > 0))
        {
            isDashing = true;
        }
    }
    private void Dash() {
        if (isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        Vector2 direction = new Vector2(xDir, yDir).normalized;//get direction of dash
        rb.AddForce(direction * DashSpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(DashTime);
        isDashing = false;
        yield return new WaitForSeconds(DashCooldown);//wait for cooldown to dash again
        canDash = true;
    }
}

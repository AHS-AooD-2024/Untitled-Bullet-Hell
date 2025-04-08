using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ProjectileInstance2D : MonoBehaviour {

    [SerializeField]
    private Projectile2D m_prototype;

    public Projectile2D prototype {get => m_prototype; }

    [SerializeField]
    private Rigidbody2D m_rigidbody2D;

    private void Awake() {
        if(m_rigidbody2D is null) {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        m_rigidbody2D.AddForce(transform.TransformDirection(Vector2.up) * m_prototype.speed, ForceMode2D.Impulse);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        other.collider.BroadcastMessage("OnHitByProjectile", this, SendMessageOptions.DontRequireReceiver);
    }

    public void Consume() {
        Destroy(gameObject);
    }
}
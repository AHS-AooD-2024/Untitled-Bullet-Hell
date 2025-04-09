using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ProjectileInstance2D : MonoBehaviour {

    [SerializeField]
    private Projectile2D m_prototype;

    /// <summary>
    /// The prototype Projectile2D that this instance is an instance of.
    /// </summary>
    public Projectile2D prototype {get => m_prototype; }

    [SerializeField]
    private Rigidbody2D m_rigidbody2D;

    private bool m_isAbleToDamage = true;

    private void Awake() {
        // The rigidbody can be set in editor to for better performance, as
        // GetComponent is quite expensive.
        if(m_rigidbody2D == null) {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }

        m_rigidbody2D.AddForce(transform.TransformDirection(Vector2.up) * m_prototype.speed, ForceMode2D.Impulse);
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(m_isAbleToDamage){
            other.BroadcastMessage("OnHitByProjectile", this, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Destroys this projectile.
    /// </summary>
    public void Consume() {
        m_isAbleToDamage = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// "Sticks" this projectile to the given game object. The projectile
    /// will become a child of the game object, will cease movement, and will
    /// no longer send OnHitByProjectile messages.
    /// <para>
    /// Note that the projectile will not move to the given game object; it only
    /// becomes its child to follow moving objects.
    /// </summary>
    /// <param name="gameObject">The game object to stick to.</param>
    public void StickTo(GameObject gameObject) {
        m_isAbleToDamage = false;
        transform.SetParent(gameObject.transform);
        m_rigidbody2D.linearVelocity = Vector2.zero;
    }
}
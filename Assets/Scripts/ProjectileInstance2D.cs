using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ProjectileInstance2D : MonoBehaviour {

    [SerializeField]
    private Projectile2D m_prototype;

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// The prototype Projectile2D that this instance is an instance of.
    /// </summary>
    public Projectile2D prototype { get => m_prototype; }

    public Alliance alliance { get => prototype.alliance; }

    public DamageInfo damage { get => prototype.damage; }

#pragma warning restore IDE1006 // Naming Styles

    [SerializeField]
    private Rigidbody2D m_rigidbody2D;

    private bool m_isAbleToDamage = true;

    private void Awake() {
        // The rigidbody can be set in editor too for better performance, as
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
    /// Destroys this projectile. It will also no longer send 
    /// <c>OnHitByProjectile</c> messages for the rest of its
    /// (albiet short) lifespan.
    /// </summary>
    public void Consume() {
        m_isAbleToDamage = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// "Sticks" this projectile to the game object of a given component. The projectile
    /// will become a child of the game object, will cease movement, and will
    /// no longer send <c>OnHitByProjectile</c> messages.
    /// <code>
    /// // These are all the same when called in a MonoBehaviour script.
    /// projectile.StickTo(this);
    /// projectile.StickTo(gameObject);
    /// projectile.StickTo(transform);
    /// </code>
    /// <para>
    /// Note that the projectile will not move to the given game object; it only
    /// becomes its child to follow moving objects.
    /// </para>
    /// </summary>
    /// <param name="component">The component of which's game object to stick to.</param>
    /// <seealso cref="StickTo(GameObject)"/>
    /// <seealso cref="StickTo(Transform)"/>
    public void StickTo(Component component) => StickTo(component.transform);

    /// <summary>
    /// "Sticks" this projectile to the given game object. The projectile
    /// will become a child of the game object, will cease movement, and will
    /// no longer send OnHitByProjectile messages.
    /// <code>
    /// // These are all the same when called in a MonoBehaviour script.
    /// projectile.StickTo(this);
    /// projectile.StickTo(gameObject);
    /// projectile.StickTo(transform);
    /// </code>
    /// <para>
    /// Note that the projectile will not move to the given game object; it only
    /// becomes its child to follow moving objects.
    /// </para>
    /// </summary>
    /// <param name="gameObject">The game object to stick to.</param>
    /// <seealso cref="StickTo(Transform)"/>
    /// <seealso cref="StickTo(Component)"/>
    public void StickTo(GameObject gameObject) => StickTo(gameObject.transform);

    /// <summary>
    /// "Sticks" this projectile to the given transform. The projectile
    /// will become a child of the transform, will cease movement, and will
    /// no longer send OnHitByProjectile messages.
    /// <code>
    /// // These are all the same when called in a MonoBehaviour script.
    /// projectile.StickTo(this);
    /// projectile.StickTo(gameObject);
    /// projectile.StickTo(transform);
    /// </code>
    /// <para>
    /// Note that the projectile will not move to the given game object; it only
    /// becomes its child to follow moving objects.
    /// </para>
    /// </summary>
    /// <param name="transform">The transform to stick to.</param>
    /// <seealso cref="StickTo(GameObject)"/>
    /// <seealso cref="StickTo(Component)"/>
    public void StickTo(Transform transform) {
        m_isAbleToDamage = false;
        this.transform.SetParent(transform);
        m_rigidbody2D.linearVelocity = Vector2.zero;
    }
}
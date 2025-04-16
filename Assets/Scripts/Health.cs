using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField]
    [Range(1.0F, 100.0F)]
    private float m_maxHealth = 1.0F;

    private float m_damageTaken = 0.0F;

    public float RemainingHealth { get => m_maxHealth - m_damageTaken; }

    public bool IsDead { get => m_damageTaken >= m_maxHealth; }
    public bool IsAlive { get => !IsDead; }

    [SerializeField]
    private Alliance[] m_takeDamageFrom;

    public void OnHitByProjectile(ProjectileInstance2D proj) {
        Debug.Log("hit");
        if(TakesDamageFrom(proj.prototype.alliance)){
            Debug.Log("Take damage");
            TakeDamage(proj.prototype.damage);
        }
    }

    public bool TakesDamageFrom(Alliance alliance) {
        foreach (Alliance a in m_takeDamageFrom) {
            if(a == alliance) {
                return true;
            }
        }
        return false;
    }

    public void OnHitBySwing(DamageInfo di) {
        if(TakesDamageFrom(di.alliance)) {
            TakeDamage(di);
        }
    }

    public void TakeDamage(DamageInfo di) {
        m_damageTaken += di.damage;
        BroadcastMessage("OnTakeDamage", di, SendMessageOptions.DontRequireReceiver);
        if(IsDead) {
            BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        }
    }
}

using System.Collections;
using Entities;
using UnityEngine;

namespace Combat {

public class Health : MonoBehaviour {
    [SerializeField]
    [Range(1.0F, 100.0F)]
    private float m_maxHealth = 1.0F;

    private float m_damageTaken = 0.0F;

    public float RemainingHealth { get => m_maxHealth - m_damageTaken; }

    public bool IsDead { get => m_damageTaken >= m_maxHealth; }
    public bool IsAlive { get => !IsDead; }

    [SerializeField]
    private Alliance m_takeDamageFrom;

    [SerializeField]
    private bool m_isFlying = false;

    [SerializeField]
    private bool m_isGrounded = false;

    public void OnHitByHazard(HazardInstance hazard) {
        print("HIT BY HAZARD");
        if(
            TakesDamageFrom(hazard.prototype.damage.alliance) && 
            (hazard.prototype.affectFlying && m_isFlying || 
            hazard.prototype.affectGrounded && m_isGrounded)
        ) {
            print("TAKING DAMAGE");
            TakeDamage(hazard.prototype.damage);

            Vector2 displacementDirection = hazard.transform.position - transform.position;
            
            transform.Translate(hazard.prototype.displacement * displacementDirection);
            // StartCoroutine(DisplaceAfterTime(1.0F, hazard));
        }
    }

    // private IEnumerator DisplaceAfterTime(float seconds, HazardInstance hazard) {
    //     yield return new WaitForSeconds(seconds);
    //     Vector2 displacementDirection = hazard.transform.position - transform.position;
            
    //     transform.Translate(hazard.prototype.displacement * displacementDirection);
    // } 

    public void OnHitByProjectile(ProjectileInstance2D proj) {
        if(TakesDamageFrom(proj.alliance)){
            TakeDamage(proj.damage);
        }
    }

    public bool TakesDamageFrom(Alliance alliance) {
        return (m_takeDamageFrom & alliance) != 0;
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

}
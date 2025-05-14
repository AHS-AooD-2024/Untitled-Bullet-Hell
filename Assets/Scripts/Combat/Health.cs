using System.Collections;
using Entities;
using Nevelson.Topdown2DPitfall.Assets.Scripts.Utils;
using UnityEngine;

namespace Combat {

public class Health : MonoBehaviour, IPitfallObject {
    [SerializeField]
    [Range(1.0F, 100.0F)]
    private float m_maxHealth = 1.0F;

    private float m_damageTaken = 0.0F;

    public float RemainingHealth { get => m_maxHealth - m_damageTaken; }
    public float MaxHealth { get => m_maxHealth; }

    public bool IsDead { get => m_damageTaken >= m_maxHealth; }
    public bool IsAlive { get => !IsDead; }

    [SerializeField]
    private Alliance m_takeDamageFrom;

    [SerializeField]
    private DamageInfo m_pitfallDamage;

    [SerializeField]
    private bool m_isFlying = false;

    [SerializeField]
    private bool m_isGrounded = false;

    [Header("I-Frames")]
    [Space]
    
    [SerializeField]
    private float m_damageGracePeriod = 0.2F;
    public float damageGracePeriod => m_damageGracePeriod;

    private float m_iframeTime = 0.0F;
    public float iframeTime => m_iframeTime;
    public float normalizedIframTime => iframeTime / damageGracePeriod;

    protected virtual void FixedUpdate() {
        if(m_iframeTime >= 0.0F) {
            m_iframeTime -= Time.fixedDeltaTime;
        }
    }

    public bool IsInIframes() {
        return m_iframeTime >= 0.0F;
    }

    public void OnHitByHazard(HazardInstance hazard) {
        if(IsInIframes()) return;

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
        if(IsInIframes()) return;
        if(TakesDamageFrom(proj.alliance)){
            TakeDamage(proj.damage);
        }
    }

    public bool TakesDamageFrom(Alliance alliance) {
        return (m_takeDamageFrom & alliance) != 0;
    }

    public void OnHitBySwing(DamageInfo di) {
        if(IsInIframes()) return;
        if(TakesDamageFrom(di.alliance)) {
            TakeDamage(di);
        }
    }

    public void TakeDamage(DamageInfo di) {
        m_iframeTime = m_damageGracePeriod;
        m_damageTaken += di.damage;
        BroadcastMessage("OnTakeDamage", di, SendMessageOptions.DontRequireReceiver);
        if(IsDead) {
            BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ResetHealth() {
        m_damageTaken = 0;
    }

    public void PitfallActionsBefore(){
    }

    public void PitfallResultingAfter(){
        TakeDamage(m_pitfallDamage);
    }
}

}
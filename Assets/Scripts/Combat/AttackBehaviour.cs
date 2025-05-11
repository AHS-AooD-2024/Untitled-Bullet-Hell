using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Combat {

public class AttackBehaviour : MonoBehaviour {
    [Header("Cooldowns")]
    [Space]

    [SerializeField]
    private float m_swingCooldownSeconds = 0.1F;
    
    // The swing cooldown is a passive cooldown, so it cannot be interrupted
    private float m_swingCooldownTime = 0.0F;

    [SerializeField]
    private float m_shootCooldownSeconds = 0.5F;

    private float m_shootCooldownTime = 0.0F;
    
    /// <summary>
    /// The number of "stages" to separate the cooldown into. If the cooldown is
    /// interrupted, it will reset to the last stage. Progress will not reset for
    /// 0 stages, and will reset to 0 for 1 stage.
    /// </summary>
    [SerializeField]
    [Range(0, 10)]
    private int m_numReloadStages = 1;

    public void Swing(float range, float breadth) => Swing(Vector2.up, range, breadth, DamageInfo.one);
    public void Swing(Vector2 direction, float range, float breadth) => Swing(direction, range, breadth, DamageInfo.one);
    public void Swing(float range, float breadth, DamageInfo damageInfo) => Swing(Vector2.up, range, breadth, DamageInfo.one);
    
    public void Swing(Vector2 direction, float range, float breadth, DamageInfo damageInfo) {
        if(IsOffCooldown){
            // m_animator.SetTrigger("On Swing");
            Vector2 fwd = transform.TransformDirection(direction);
            Vector2 pos2D = transform.position;
            Vector2 swingFrom = 0.5F * range * fwd + pos2D;
            float radius = breadth * 0.5F;
            Vector2 boxSize = new(breadth, range * 0.5F);

            Collider2D[] hits = Physics2D.OverlapBoxAll(swingFrom, boxSize, Vector2.SignedAngle(fwd, Vector2.up));
            Debug.DrawLine(swingFrom, swingFrom + range * fwd, Color.white, 1.0F);
            Debug.DrawLine(swingFrom - Vector2.Perpendicular(fwd) * breadth, swingFrom + Vector2.Perpendicular(fwd) * breadth, Color.white, 1.0F);
            foreach (Collider2D hit in hits) {
                hit.BroadcastMessage("OnHitBySwing", damageInfo, SendMessageOptions.DontRequireReceiver);
            }
            m_swingCooldownTime = m_swingCooldownSeconds;
        }
    }

    public void Shoot(Projectile2D projectile, Vector2 direction, Vector2 offset) {
        if(IsOffCooldown) {
            projectile.Launch(transform.position + transform.TransformDirection(offset), transform.TransformDirection(direction));
            m_shootCooldownTime = m_shootCooldownSeconds;
        }
    }

    public void UpdateCooldowns(float deltaTime) {
        if(m_swingCooldownTime > 0.0F) m_swingCooldownTime -= deltaTime;
        if(m_shootCooldownTime > 0.0F) m_shootCooldownTime -= deltaTime;
        // m_animator.SetFloat("Swing Cooldown Time", m_swingCooldownTime);
        // m_animator.SetFloat("Shoot Cooldown Time", m_shootCooldownTime);
    }
    
    public void Shoot(Projectile2D projectile) => Shoot(projectile, Vector2.up, Vector2.zero);
    public void Shoot(Projectile2D projectile, Vector2 direction) => Shoot(projectile, direction, Vector2.zero);

    public bool IsOnCooldown { get => m_swingCooldownTime > 0.0F || m_shootCooldownTime > 0.0F; }
    public bool IsOffCooldown { get => !IsOnCooldown; }

    public void InterruptReload() {
        // Example:
        // cooldown = 1.0
        // time = 0.6
        // numStages = 3
        //
        // dt = 1.0 / 3 = 0.33
        // progress = 1.0 - 0.6 = 0.4
        // done = floor(0.4 / 0.33) = floor(1.2) = 1.0
        // cp = 1.0 * 0.33 = 0.33
        // final = 1.0 - 0.33 = 0.67
        // 
        // Someone double check my math please -- S.
        float dt = m_shootCooldownSeconds / m_numReloadStages;
        float progress = m_shootCooldownSeconds - m_shootCooldownTime;
        float stagesDone = Mathf.Floor(progress / dt);
        float checkpoint = stagesDone * dt;
        m_shootCooldownTime = m_shootCooldownSeconds - checkpoint;
    }
}

}

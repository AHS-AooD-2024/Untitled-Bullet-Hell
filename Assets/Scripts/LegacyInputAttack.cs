using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyInputAttack : AttackBehaviour {

    [Space]

    [SerializeField]
    private LookingGlass m_lookingGlass;

    [Header("Ranged")]
    [Space]

    [SerializeField]
    private Projectile2D m_projectile;

    [SerializeField]
    private bool m_doAutoFire = true;

    private bool m_doShoot = false;

    [SerializeField]
    private Vector2 m_relativeOffset = Vector2.zero;

    [Header("Melee")]
    [Space]

    [SerializeField]
    private float m_range = 1.0F;

    [SerializeField]
    private float m_breadth = 1.0F;

    [SerializeField]
    private DamageInfo m_meleeDamage;

    [SerializeField]
    private bool m_doAutoSwing = true;

    private bool m_doSwing = false;

    private void Update() {
        if(m_doAutoSwing && Input.GetButton("Fire1") || Input.GetButtonDown("Fire1")) {
            m_doSwing = true;
        }
        
        if(m_doAutoFire && Input.GetButton("Fire2") || Input.GetButtonDown("Fire2")) {
            m_doShoot = true;
        }
    }

    private void FixedUpdate() {
        UpdateCooldowns(Time.fixedDeltaTime);

        if(m_doSwing) {
            Swing(m_range, m_breadth);
            m_doSwing = false;
        }

        if(m_doShoot) {
            Shoot(m_projectile, m_lookingGlass.Direction, m_relativeOffset);
            m_doShoot = false;
        }
    }

    public void OnDash() {
        InterruptReload();
        // prevent attacks during dash
        m_doShoot = false;
        m_doSwing = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyInputAttack : AttackBehaviour {
    [Header("Ranged")]
    [Space]

    [SerializeField]
    private Projectile2D m_projectile;

    [SerializeField]
    private Vector2 m_relativeOffset = Vector2.zero;

    [Header("Melee")]
    [Space]

    [SerializeField]
    private float m_range = 1.0F;

    [SerializeField]
    private float m_breadth = 1.0F;

    private bool m_doSwing = false;
    private bool m_doShoot = false;

    private void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Debug.Log("Swing");
            m_doSwing = true;
        }
        
        if(Input.GetButtonDown("Fire2")) {
            Debug.Log("Shoot");
            m_doShoot = true;
        }
    }

    private void FixedUpdate() {
        if(m_doSwing) {
            Swing(m_range, m_breadth);
            m_doSwing = false;
        }

        if(m_doShoot) {
            Shoot(m_projectile, m_relativeOffset);
            m_doShoot = false;
        }
    }
}

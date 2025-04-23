using System;
using UnityEngine;

public class ConstantShootingAttack : AttackBehaviour {

    [SerializeField]
    private Projectile2D m_projectile;

    [SerializeField]
    private LookingGlass m_lookingGlass;

    private void FixedUpdate() {
        UpdateCooldowns(Time.fixedDeltaTime);
        Shoot(m_projectile, m_lookingGlass.Direction);
    }
}
using UnityEngine;

public class ConstantShootingAttack : AttackBehaviour {

    [SerializeField]
    private Projectile2D m_projectile;

    private void FixedUpdate() {
        UpdateCooldowns(Time.fixedDeltaTime);
        Shoot(m_projectile);
    }
}
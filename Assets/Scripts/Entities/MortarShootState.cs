using Entities.States;
using UnityEngine;

namespace Entities {

    public class MortarShootState : EntityState {

        private readonly float m_shootInterval = 1.0F;

        private readonly TargetIndicator m_targetIndicator;

        private readonly Projectile2D m_projectile;

        private float m_nextShootTime = 0.0F;

        public MortarShootState(float shootInterval, TargetIndicator targetIndicator, Projectile2D projectile) {
            m_shootInterval = shootInterval;
            m_targetIndicator = targetIndicator;
            m_nextShootTime = m_shootInterval;
            m_projectile = projectile;
        }

        protected override void OnEnter() {
        }

        protected override void OnExit() {
        }

        protected override void OnUpdate() {
            if(age > m_nextShootTime) {
                m_nextShootTime += m_shootInterval;
                PlayAnimation("Shoot");

                TargetIndicator tg = Object.Instantiate(m_targetIndicator, player.transform.position, Quaternion.identity);
                tg.corrospondingProjectile = m_projectile.Launch((Vector2) player.transform.position + 5.0F * Vector2.up, Vector2.down);
            }
        }
    }

}
using Entities.States;
using UnityEngine;

namespace Entities {
    public class MortarStateMachine : StateMachine {
        [SerializeField]
        private float m_shootInterval;

        [SerializeField]
        [Tooltip("How far above the player to spawn projectiles")]
        private float m_distanceToFall;

        [SerializeField]
        private TargetIndicator m_targetIndicator;

        [SerializeField]
        private Projectile2D m_projectile;


        protected override EntityState GetInitialState() {
            return new MortarShootState(m_shootInterval, m_targetIndicator, m_projectile);
        }
    }
}
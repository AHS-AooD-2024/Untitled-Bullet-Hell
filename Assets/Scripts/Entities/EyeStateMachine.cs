using Entities.States;
using UnityEngine;

namespace Entities {
    public class EyeStateMachine : StateMachine {
        [SerializeField]
        private float m_sightRange = 10.0F;

        [SerializeField]
        private float m_shootInterval = 1.0F;

        [SerializeField]
        private float m_distanceFromPlayer = 1.0F;

        [SerializeField]
        private float m_revolutionaryPeriod = 1.0F;

        [SerializeField]
        private Projectile2D m_bullet;

        protected override EntityState GetInitialState() {
            return new SearchState(m_sightRange, new EyeCircleState(m_shootInterval, m_distanceFromPlayer, m_revolutionaryPeriod, m_bullet));
        }

        protected override EntityState GetDeathState() {
            return new DeathState();
        }
    }
}
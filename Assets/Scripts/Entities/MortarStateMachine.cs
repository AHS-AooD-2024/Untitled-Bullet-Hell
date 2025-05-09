using Entities.States;
using UnityEngine;

namespace Entities {
    public class MortarStateMachine : StateMachine {
        [SerializeField]
        private float m_shootInterval;

        [SerializeField]
        private TargetIndicator m_targetIndicator;

        [SerializeField]
        private Projectile2D m_projectile;


        protected override EntityState GetInitialState() {
            return new MortarShootState(m_shootInterval, m_targetIndicator, m_projectile);
        }
    }
}
using UnityEngine;

namespace Entities.States {
    public class DeathState : EntityState {
        private AnimatorStateInfo m_asi;

        protected override void OnEnter() {
            PlayAnimation("Die");
            m_asi = animator.GetCurrentAnimatorStateInfo(0);
        }
    
        protected override void OnExit() {
        }
    
        protected override void OnUpdate() {
            if(age > m_asi.length) {
                Object.Destroy(gameObject);
            }
        }
    }
}
using UnityEngine;

namespace Entities {
    public class AnimationHoleFalling : HoleFallableBehaviour {
        [SerializeField]
        private Animator m_animator;
        
        private float m_fallLength = -1.0F;

        private static readonly int m_fallHash = Animator.StringToHash("Fall");

        protected override void Fall() {
            if(m_animator.HasState(0, m_fallHash)) {
                m_animator.PlayInFixedTime(m_fallHash);

                if(m_fallLength < 0.0F) {
                    m_fallLength = m_animator.GetCurrentAnimatorStateInfo(0).length;
                }

                StartCoroutine(Respawn(m_fallLength, positionCache2D.EarliestFrame));
            } else {
                Debug.LogWarning("No \"Fall\" animation found");
            }
        }
    }
}
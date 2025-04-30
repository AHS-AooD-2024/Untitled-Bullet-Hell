using System.Collections;
using UnityEngine;
using Util;

namespace Entities {
    [RequireComponent(typeof(PositionCache2D))]
    public class HoleFallableBehaviour : MonoBehaviour {
        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private TopDownCharacterControllerBehaviour m_controller;
        private static readonly int m_fallHash = Animator.StringToHash("Fall");

        private float m_fallLength = -1.0F;

        [SerializeField]
        private PositionCache2D m_posCache;

        public void OnHitByHazard(HazardInstance hazard) {
            // we are immune to falling while dashing/dodge-rolling
            if(hazard is Hole && !m_controller.IsDashing) {
                if(m_animator.HasState(0, m_fallHash)) {
                    m_animator.PlayInFixedTime(m_fallHash);

                    if(m_fallLength < 0.0F) {
                        m_fallLength = m_animator.GetCurrentAnimatorStateInfo(0).length;
                    }

                    StartCoroutine(Respawn(m_fallLength, m_posCache.EarliestFrame));
                } else {
                    Debug.LogWarning("No \"Fall\" animation found");
                    StartCoroutine(Respawn(1.0F, m_posCache.EarliestFrame));
                }
            }
        }

        private IEnumerator Respawn(float afterSeconds, Vector2 atPosition) {
            m_controller.Stop();
            m_controller.enabled = false;
            yield return new WaitForSeconds(afterSeconds);
            m_controller.enabled = true;
            transform.position = atPosition;
        }
    }
}
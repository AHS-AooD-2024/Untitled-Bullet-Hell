using System.Collections;
using UnityEngine;
using Util;

namespace Entities {
    [RequireComponent(typeof(PositionCache2D))]
    public abstract class HoleFallableBehaviour : MonoBehaviour {
        [SerializeField]
        private TopDownCharacterControllerBehaviour m_controller;

        [SerializeField]
        private PositionCache2D m_posCache;

        protected PositionCache2D positionCache2D => m_posCache;

        public void OnHitByHazard(HazardInstance hazard) {
            // we are immune to falling while dashing/dodge-rolling
            if(hazard is Hole && !m_controller.IsDashing) {
                Fall();
            }
        }

        protected void StopMoving() {
            m_controller.Stop();
        }

        protected bool canMove { get => m_controller.enabled; set => m_controller.enabled = value; }

        protected abstract void Fall();

        protected IEnumerator Respawn(float afterSeconds, Vector2 atPosition) {
            yield return new WaitForSeconds(afterSeconds);
            transform.position = atPosition;
        }
    }
}
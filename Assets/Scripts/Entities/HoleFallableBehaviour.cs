using System.Collections;
using UnityEngine;
using Util;

namespace Entities {
    [RequireComponent(typeof(PositionCache2D))]
    public abstract class HoleFallableBehaviour : MonoBehaviour {
        [SerializeField]
        private TopDownCharacterControllerBehaviour m_controller;

        [SerializeField]
        private Collider2D m_feetHitbox;

        [SerializeField]
        private PositionCache2D m_posCache;

        protected PositionCache2D positionCache2D => m_posCache;

        private bool m_isTryingToFall = false;

        public void OnHitByHazard(HazardInstance hazard) {
            // we are immune to falling while dashing/dodge-rolling
            if(!m_isTryingToFall && hazard is Hole && !m_controller.IsDashing) {
                Hole hole = hazard as Hole;
                if(CanFall(hole)) {
                    Fall();
                } else {
                    m_isTryingToFall = true;
                    StartCoroutine(KeepTryingToFall(hole));
                }
            }
        }

        // Without the distance check, taller hitboxes can fall into holes when the feet
        // are clearly on the ground. This helps minimize that problem.
        // Another solution would be to skew the hitbox up slightly. Actually, that's probably
        // better... oh well.
        private IEnumerator KeepTryingToFall(Hole hole) {
            do {
                yield return new WaitForFixedUpdate();
            } while (!CanFall(hole));
            Fall();
            m_isTryingToFall = false;
        }

        private bool CanFall(Hole hole) {
            // return Vector2.Distance(hole.transform.position, transform.position) <= hole.fallWithinDistance;
            return hole.hitbox.IsTouching(m_feetHitbox);
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
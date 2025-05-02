using System.Collections;
using UnityEngine;

namespace Entities {
    public class ScaleHoleFalling : HoleFallableBehaviour {
        [Header("Scale")]
        [Space]

        [SerializeField]
        private float m_scaleDownDuration = 1.0F;

        [SerializeField]
        private float m_respawnDelay = 0.0F;

        private Vector3 m_defaultScale;
        private Vector3 m_minScale = Vector3.zero;

        private void Awake() {
            m_defaultScale = transform.localScale;
            m_minScale.z = m_defaultScale.z;
        }

        protected override void Fall() {
            StartCoroutine(ScaleThenRespawn());
        }

        private IEnumerator ScaleThenRespawn() {
            StopMoving();
            canMove = false;
            // Get the position here so that it will be relative to when the player fell
            // otherwise the player might get stuck if the scale duration is longer than
            // the posCache can see back.
            Vector2 safePosition = positionCache2D.EarliestFrame;
            yield return CrossFadeScale(m_defaultScale, m_minScale, m_scaleDownDuration);
            yield return Respawn(m_respawnDelay, safePosition);
            transform.localScale = m_defaultScale;
            canMove = true;
        }

        private IEnumerator CrossFadeScale(Vector2 from, Vector2 to, float duration) {
            float z = transform.localScale.z;
            return CrossFadeScale(new Vector3(from.x, from.y, z), new Vector3(to.x, to.y, z), duration);
        }

        private IEnumerator CrossFadeScale(Vector3 from, Vector3 to, float duration) {
            float t = 0.0F;
            while(t < duration) {
                t += Time.deltaTime;
                transform.localScale = Vector3.Lerp(from, to, t / duration);
                yield return null;
            }

            transform.localScale = to;
        }
    }
}
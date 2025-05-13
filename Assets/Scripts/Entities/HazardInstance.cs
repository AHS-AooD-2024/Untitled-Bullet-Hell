using Combat;
using UnityEngine;

namespace Entities {
    public class HazardInstance : MonoBehaviour {
        [SerializeField]
        private Hazard m_hazard;

        [SerializeField]
        private Collider2D m_hitbox;

        public Hazard prototype => m_hazard;
        public Collider2D hitbox => m_hitbox;

        public DamageInfo damage => prototype.damage;

        private void OnTriggerEnter2D(Collider2D collider) {
            collider.BroadcastMessage("OnHitByHazard", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
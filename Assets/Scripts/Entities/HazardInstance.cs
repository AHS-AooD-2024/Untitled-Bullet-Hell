using Combat;
using UnityEngine;

namespace Entities {
    public class HazardInstance : MonoBehaviour {
        [SerializeField]
        private Hazard m_hazard;

        public Hazard prototype => m_hazard;

        private void OnTriggerEnter2D(Collider2D collider) {
            collider.BroadcastMessage("OnHitByHazard", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}
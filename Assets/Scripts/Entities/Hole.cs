using UnityEngine;

namespace Entities {
    public class Hole : HazardInstance {
        [SerializeReference]
        [Tooltip("How close the collider position has to be to fall.")]
        private float m_fallWithin = 0.5F;

        public float fallWithinDistance => m_fallWithin;
    }
}
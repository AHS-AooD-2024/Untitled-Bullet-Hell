using Combat;
using UnityEngine;


namespace Entities {
    
    [CreateAssetMenu(fileName = "Hazard", menuName = "Hazard", order = 0)]
    public class Hazard : ScriptableObject {
        [SerializeField]
        private DamageInfo m_damage;

        [SerializeField]
        private float m_displacement = 0.0F;

        [SerializeField]
        private bool m_affectGrounded = true;

        [SerializeField]
        private bool m_affectFlying = true;

        public DamageInfo damage => m_damage;
        public float displacement => m_displacement;

        public bool affectFlying => m_affectFlying;
        public bool affectGrounded => m_affectGrounded;
    }
}
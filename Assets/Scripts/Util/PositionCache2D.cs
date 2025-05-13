using System.Collections.Generic;
using UnityEngine;

namespace Util {    
    public class PositionCache2D : MonoBehaviour {
        [SerializeField]
        private uint m_size;

        private readonly LinkedList<Vector2> m_positions = new();

        private void FixedUpdate() {
            m_positions.AddLast(transform.position);
            if(m_positions.Count >= m_size) {
                m_positions.RemoveFirst();
            }
        }

        public Vector2 LastFrame => m_positions.Last.Value;
        public Vector2 EarliestFrame => m_positions.First.Value;
    }
}
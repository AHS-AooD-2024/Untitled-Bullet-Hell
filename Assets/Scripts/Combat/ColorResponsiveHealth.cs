using System.Collections;
using UnityEngine;

namespace Combat {    
    public class ColorResponsiveHealth : Health {
        [SerializeField]
        private SpriteRenderer m_spriteRenderer;

        [SerializeField]
        private Color m_damageColor = Color.red;

        private Color m_defaultColor = Color.white;

        [SerializeField]
        private float m_colorFadeDuration = 0.2F;

        public void OnTakeDamage() {
            if(IsAlive) {
                m_defaultColor = m_spriteRenderer.color;
                m_spriteRenderer.color = m_damageColor;
                StartCoroutine(CrossFade(m_damageColor, m_defaultColor));   
            }
        }

        private IEnumerator CrossFade(Color from, Color to) {
            float time = 0.0F;
            while(time < m_colorFadeDuration) {
                time += Time.deltaTime;
                m_spriteRenderer.color = Color.Lerp(from, to, time / m_colorFadeDuration);
                yield return null;
            }
            m_spriteRenderer.color = to;
        }
    }
}
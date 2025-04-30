using System.Collections;
using UnityEngine;

namespace Combat {    
    public class ColorResponsiveHealth : Health {
        [SerializeField]
        private float m_iframeFlashInterval = 0.0F;

        [SerializeField]
        private SpriteRenderer m_spriteRenderer;

        [SerializeField]
        private Color m_damageColor = Color.red;

        private Color m_defaultColor = Color.white;

        [SerializeField]
        private float m_colorFadeDuration = 0.2F;

        protected virtual void Awake() {
            if(m_spriteRenderer == null) {
                m_spriteRenderer = GetComponent<SpriteRenderer>();
            }

            m_defaultColor = m_spriteRenderer.color;
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            if(m_iframeFlashInterval > 0.0F && IsInIframes()) {
                float alpha = 1.0F - Mathf.PingPong(normalizedIframTime / m_iframeFlashInterval * 2.0F, 1.0F);
                m_spriteRenderer.color = new(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, alpha);
            }
        }

        public void OnTakeDamage() {
            if(IsAlive) {
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
using UnityEngine;

public class PlayerHealth : Health {
    [SerializeField]
    private Color m_deadColor;

    [SerializeField]
    private SpriteRenderer m_spriteRenderer;

    private void Awake() {
        if(m_spriteRenderer == null) {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void OnDeath() {
        m_spriteRenderer.color = m_deadColor;
        Debug.Log("I am dead!");
    }
}
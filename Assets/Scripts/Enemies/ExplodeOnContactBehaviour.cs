using UnityEngine;
using Util;

public class ExplodeOnContactBehaviour : MonoBehaviour {
    [SerializeField]
    private DamageInfo m_explosionDamage = DamageInfo.one;

    [SerializeField]
    private float m_explosionRadius = 1.0F;

    // might change to trigger, idk
    private void OnCollisionEnter2D(Collision2D other) {
        Explode();
    }

    public void Explode() {
        Debug.Log("BOOOM");
        Gizmos2D.DrawCircle(transform.position, m_explosionRadius, 1.0F);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, m_explosionRadius);
        foreach (Collider2D hit in hits) {
            hit.BroadcastMessage("OnHitBySwing", m_explosionDamage, SendMessageOptions.DontRequireReceiver);
        }
    }
}

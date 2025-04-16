using UnityEngine;

public class AlwaysLookAtBehaviour : MonoBehaviour {
    [SerializeField]
    private GameObject m_lookAt;

    private void FixedUpdate() {
        Vector2 lookDir = m_lookAt.transform.position - transform.position;
        transform.eulerAngles = Vector3.forward * Vector2.SignedAngle(Vector2.up, lookDir);
    }
}
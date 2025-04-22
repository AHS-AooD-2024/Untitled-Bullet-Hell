using System;
using UnityEngine;

public class LOSLookAtBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject m_lookAt;

    [Header("LOS Checking")]
    [Space]

    [SerializeField]
    private ContactFilter2D m_LineOfSightFilter;

    // Avoid constant allocs in FixedUpdate
    private readonly RaycastHit2D[] m_hits = new RaycastHit2D[4];

    private void FixedUpdate() {
        // TODO: make this more efficient by doing a filtered linecast instead of 
        // filtering after the fact.
        int n = Physics2D.Linecast(
            m_lookAt.transform.position, transform.position,
            m_LineOfSightFilter, m_hits
        );

        bool doLook = true;
        for(int i = 0; i < n; i++) {
            if(OtherThanUs(m_hits[i])) {
                // print(m_hits[i].transform.name);
                doLook = true;
                break;
            }
        }

        if(doLook) {
            Look();
        }
    }

    private void Look() {
        Vector2 lookDir = m_lookAt.transform.position - transform.position;
        transform.eulerAngles = Vector3.forward * Vector2.SignedAngle(Vector2.up, lookDir);
    }

    private bool OtherThanUs(RaycastHit2D hit) {
        return hit.transform != transform && hit.transform != m_lookAt.transform;
    }
}

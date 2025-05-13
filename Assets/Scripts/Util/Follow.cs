using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util {
    
public class Follow : MonoBehaviour {
    [SerializeField]
    private bool m_followXPosition;

    [SerializeField]
    private bool m_followYPosition;

    [SerializeField]
    private bool m_followZPosition;

    [SerializeField]
    private bool m_followRotation;

    [SerializeField]
    private GameObject m_followee;

    // Update is called once per frame
    void Update() {
        if(m_followXPosition || m_followYPosition || m_followZPosition) {
            Vector3 setpos = transform.position;
            Vector3 folpos = m_followee.transform.position;
            if(m_followXPosition) setpos.x = folpos.x;
            if(m_followYPosition) setpos.y = folpos.y;
            if(m_followZPosition) setpos.z = folpos.z;
            transform.position = setpos;
        }

        if(m_followRotation) {
            transform.rotation = m_followee.transform.rotation;
        }
    }
}

}
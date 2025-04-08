using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyInputAttack : AttackBehaviour {

    [SerializeField]
    private float m_range = 1.0F;

    [SerializeField]
    private float m_breadth = 1.0F;

    private bool m_doSwing = false;

    private void Update() {
        if(Input.GetButtonDown("Fire1")) {
            Debug.Log("Swing");
            m_doSwing = true;
        }
    }

    private void FixedUpdate() {
        if(m_doSwing) {
            Swing(m_range, m_breadth);
            m_doSwing = false;
        }
    }
}

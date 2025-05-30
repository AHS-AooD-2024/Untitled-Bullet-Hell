using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyInputCharacterController : TopDownCharacterControllerBehaviour {

    /// <summary>
    /// Whether the charcter look should follow the mouse.
    /// Enable for Enter the Gungeon style, Disable for Binding of Isaac style.
    /// </summary>
    [SerializeField]
    private bool m_doFollowMouse = false;
    
    private Vector2 m_input = Vector2.zero;
    private Vector2 m_look = Vector2.zero;

    private bool m_doDash = false;

    protected override void Awake() {
        base.Awake();

        // We will confine the mouse if we are actually using it.
        // Technically, we could do this in update in editor so
        // that disabling follow mouse changes the lock state,
        // but this might change to support a custom mouse.
        // We also might just hide the mouse and forego aiming
        // BoI style. 
        if(m_doFollowMouse){
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void Update() {
        if(m_doFollowMouse) {
            int w = Screen.width;
            int h = Screen.height;

            // We need the mouse pos relative to the player, but the mouse pos
            // and player pos are measured in wildly different spaces. So, we have
            // to convert.
            // Despite these being `Vector3`s, their z components should always be 0
            // Vector3 ourPos = Camera.main.transform.position - transform.position;
            // Nevermind, this doesn't actually work.
            Vector3 mousePxSpace = Input.mousePosition;
            Vector3 mouseGameSpace = new(mousePxSpace.x - w / 2, mousePxSpace.y - h / 2);
            // m_look = mouseGameSpace - ourPos;
            m_look = mouseGameSpace;
        } 
        // If we are in the editor, m_doFollowMouse is able to change. In build, it
        // will never change, so this is actually not needed.
        #if UNITY_EDITOR
        else {
            m_look.x = 0.0F;
            m_look.y = 0.0F;
        }
        #endif

        m_input.x = Input.GetAxisRaw("Horizontal");
        m_input.y = Input.GetAxisRaw("Vertical");
        if( m_input != Vector2.zero && Input.GetButtonDown("Jump")) {
            m_doDash = true;
        }
    }

    void FixedUpdate() {
        Move(m_input, m_look, m_doDash, Time.fixedDeltaTime);
        m_doDash = false;
    }
}

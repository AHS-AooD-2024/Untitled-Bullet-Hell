using UnityEngine;

namespace Entities.States {

[RequireComponent(typeof(Animator), typeof(Collider2D), typeof(ITopDownCharacterController))]
public abstract class StateMachine : MonoBehaviour {
    private EntityState m_state;

    [SerializeField]
    private Collider2D m_collider;

    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private Collider2D m_player;

    // This is just for if something uses the default behavior I wrote
    // btw, we might want to do an enemy controller?
    [SerializeField]
    private TopDownCharacterControllerBehaviour m_controller;
    // it can't be serialized as an interface ,_,
    private ITopDownCharacterController m_realController;

    protected abstract EntityState GetInitialState();

    private void Awake() {
        m_state = GetInitialState();

        if(m_animator == null) {
            m_animator = GetComponent<Animator>();
        }

        if(m_player == null) {
            m_player = GameObject.Find("Player").GetComponent<Collider2D>();
        }

        if(m_collider == null) {
            m_collider = GetComponent<Collider2D>();
        }

        if(m_controller == null) {
            m_realController = GetComponent<ITopDownCharacterController>();
        } else {
            m_realController = m_controller;
        }

        m_state.Init(new EntityState.StateInfo {
            gameObject = gameObject, 
            animator = m_animator, 
            collider = m_collider, 
            player = m_player.gameObject, 
            playerCollider = m_player,
            controller = m_realController,
        });
    }

    private void FixedUpdate() {
        m_state.Update(Time.fixedDeltaTime);
        if(m_state.hasNext) {
            m_state.Exit();
            m_state = m_state.next;
            m_state.Enter();
        }
    }
}

}
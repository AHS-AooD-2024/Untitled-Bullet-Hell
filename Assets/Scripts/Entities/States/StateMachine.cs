using UnityEngine;
using UnityEngine.Events;

namespace Entities.States {

[RequireComponent(typeof(Collider2D), typeof(ITopDownCharacterController))]
[RequireComponent(typeof(LookingGlass))]
public abstract class StateMachine : MonoBehaviour {

    [Header("Events")]
    [Space]

    [SerializeField] 
    private UnityEvent m_onDeath;

    public UnityEvent onDeath => m_onDeath;

    [Header("Info")]
    [Space]

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

    [SerializeField]
    private LookingGlass m_lookingGlass;

    private EntityState.StateInfo m_info;

    protected Collider2D player { get => m_info.player; }

    /// <summary>
    /// Gets the state that a state machine entity has at first. This will
    /// only be called once and is the entrence into the state chain.
    /// </summary>
    /// <returns>The initial state of this entity</returns>
    protected abstract EntityState GetInitialState();

    protected virtual EntityState GetDeathState() {
        return new DeathState();
    }

    private void Awake() {
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

        if(m_lookingGlass == null) {
            m_lookingGlass = GetComponent<LookingGlass>();
        }

        m_info = new EntityState.StateInfo {
            gameObject = gameObject, 
            animator = m_animator, 
            collider = m_collider,
            player = m_player,
            controller = m_realController,
            lookingGlass = m_lookingGlass,
        };
        
        m_state = GetInitialState();

        m_state.Init(m_info);
    }

    private void FixedUpdate() {
        m_state.Update(Time.fixedDeltaTime);
        if(m_state.hasNext) {
            m_state.Exit();
            m_state = m_state.next;
            m_state.Enter();
        }
    }

    public void OnDeath() {
        m_onDeath.Invoke();
        m_state.Exit();
        m_state = GetDeathState();
        m_state.Init(m_info);
        m_state.Enter();
    }
}

}
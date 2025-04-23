using System;
using UnityEngine;

namespace Entities.States {
public abstract class EntityState {
    // DO NOT DO READONLY STRUCT. It is important that StateInfo is a reference type
    // so that chained states can pass the reference around rather than copying a value
    // type.
    public sealed class StateInfo {
        #if CSHARP_11_OR_LATER
        required
        #endif
        public GameObject gameObject { get; init; }

        #if CSHARP_11_OR_LATER
        required
        #endif
        public Collider2D collider { get; init; }

        #if CSHARP_11_OR_LATER
        required
        #endif
        public GameObject player { get; init; }

        #if CSHARP_11_OR_LATER
        required
        #endif
        public Collider2D playerCollider { get; init; }

        #if CSHARP_11_OR_LATER
        required
        #endif
        public Animator animator { get; init; }
        
        #if CSHARP_11_OR_LATER
        required
        #endif
        public ITopDownCharacterController controller { get; init; }
    }

    private StateInfo m_info;

    private EntityState m_next;
    private EntityState m_prev;
    
    // view methods into info
    protected Animator animator { get => m_info.animator; }
    protected GameObject gameObject { get => m_info.gameObject; }
    protected Transform transform { get => gameObject.transform; }
    protected Collider2D collider { get => m_info.collider; }
    protected Collider2D playerCollider { get => m_info.playerCollider; }
    protected ITopDownCharacterController controller { get => m_info.controller; }

    private float m_age = 0.0F;
    protected float age { get => m_age; }
    private float m_deltaTime = 0.0F;
    protected float deltaTime { get => m_deltaTime; }

    public EntityState next { get => m_next; }
    public bool hasNext { get => m_next != null; }

    public EntityState prev { get => m_next; }
    public bool hasPrev { get => m_prev != null; }

    private bool m_initFlag;
    public void Init(StateInfo info) {
        if(m_initFlag) {
            throw new InvalidOperationException("Do not initialize a state twice.");
        }

        Imprint(m_info);
    }
    
    private void Imprint(StateInfo info) {
        m_initFlag = true;
        m_info = info;
    }

    public void Update(float deltaTime) {
        m_age += deltaTime;
        m_deltaTime = deltaTime;
        OnUpdate();
    }

    public void Enter() {
        OnEnter();
    }

    public void Exit() {
        OnExit();
    }

    protected abstract void OnEnter(); 
    protected abstract void OnExit(); 
    protected abstract void OnUpdate();

    protected void Then(EntityState state) {
        m_next = state;
        m_next.m_prev = this;
        m_next.Imprint(m_info);
    }

    protected void SetWaypoint(Vector2 position, float timeout = Mathf.Infinity) {

    }

    protected void SetWaypoint(Transform follow, float targetDistance = 0.0F, float timeout = Mathf.Infinity) {

    }

    protected IdleState IdleFor(float seconds, string animation) {
        return IdleState.ForSeconds(seconds, animation);
    }

    protected void KickBack() {
        Then(prev);
    }

    public void PlayAnimation(string name) {
        animator.PlayInFixedTime(name);
    }
}

}
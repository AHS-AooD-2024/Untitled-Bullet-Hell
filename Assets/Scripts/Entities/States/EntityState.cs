using System;
using System.Collections.Generic;
using Combat;
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
        public Collider2D player { get; init; }

        #if CSHARP_11_OR_LATER
        required
        #endif
        public Animator animator { get; init; }
        
        #if CSHARP_11_OR_LATER
        required
        #endif
        public ITopDownCharacterController controller { get; init; }

        #if CSHARP_11_OR_LATER
        required
        #endif
        public LookingGlass lookingGlass { get; init; }
    }

    private StateInfo m_info;

    private EntityState m_next;
    private EntityState m_prev;
    
    // view methods into info

    /// <summary>
    /// The <see cref="Animator"/> controlling the sprite of the entity this state controls.
    /// </summary>
    protected Animator animator { get => m_info.animator; }

    /// <summary>
    /// The <see cref="GameObject"/> entity this state controls.
    /// </summary>
    protected GameObject gameObject { get => m_info.gameObject; }

    /// <summary>
    /// The <see cref="Transform"/> of this state's entity. This is equavilant to
    /// <c>gameObject.transform</c>.
    /// </summary>
    /// <remarks>
    /// Note that <c>transform.rotation</c> is not useful to the direction an entity
    /// may be facing. Use <see cref="look"/> for getting the direction an entity
    /// is facing.
    /// </remarks>
    protected Transform transform { get => gameObject.transform; }

    /// <summary>
    /// The <see cref="Collider2D"/> of this state's entity. Every entity that can
    /// be controlled by a state machine is required to have a collider.
    /// </summary>
    protected Collider2D collider { get => m_info.collider; }

    /// <summary>
    /// The player's <see cref="Collider2D"/>.
    /// </summary>
    protected Collider2D player { get => m_info.player; }

    /// <summary>
    /// A controller that can move this state's entity. This will never be <c>null</c>, 
    /// even if the entity doesn't/cannot move.
    /// </summary>
    protected ITopDownCharacterController controller { get => m_info.controller; }

    /// <summary>
    /// A supplier of look direction that allows for entities to avoid rotating their transform.
    /// </summary>
    /// <seealso cref="LookingGlass"/>
    protected LookingGlass look { get => m_info.lookingGlass; }

    private float m_age = 0.0F;

    /// <summary>
    /// The amount of time, in seconds, that this state has been active for.
    /// </summary>
    protected float age { get => m_age; }

    private float m_deltaTime = 0.0F;

    /// <summary>
    /// The amount of time, in seconds, that has passed between the last call to
    /// <see cref="OnUpdate"/>
    /// </summary>
    protected float deltaTime { get => m_deltaTime; }

    /// <summary>
    /// The state that will be moved to next. If the state is not finished, 
    /// this will be <c>null</c>
    /// </summary>
    public EntityState next { get => m_next; }

    /// <summary>
    /// <c>true</c> if the state is finished, <c>false</c> otherwise.
    /// </summary>
    public bool hasNext { get => m_next != null; }

    /// <summary>
    /// The state that happened before this state started.
    /// </summary>
    public EntityState prev { get => m_prev; }

    /// <summary>
    /// <c>true</c> if this was not the initial state, <c>false</c> otherwise.
    /// </summary>
    public bool hasPrev { get => m_prev != null; }

    private bool m_initFlag;
    public void Init(StateInfo info) {
        if(m_initFlag) {
            throw new InvalidOperationException("Do not initialize a state twice.");
        }

        Imprint(info);
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
        // states can be entered more than once, so resetting is needed
        m_age = 0.0F;
        m_next = null;
        OnEnter();
    }

    public void Exit() {
        OnExit();
    }

    protected abstract void OnEnter(); 
    protected abstract void OnExit(); 
    protected abstract void OnUpdate();

    /// <summary>
    /// Finishes this state and sets the next state.
    /// </summary>
    /// <param name="state">The state that will be entered after this state exits.</param>
    protected void Then(EntityState state) {
        m_next = state ?? throw new ArgumentNullException(nameof(state));
        m_next.m_prev = this;
        m_next.Imprint(m_info);
    }

    // TODO: these
    protected void SetWaypoint(Vector2 position, float timeout = Mathf.Infinity) {
        throw new NotImplementedException();
    }

    protected void SetWaypoint(Transform follow, float targetDistance = 0.0F, float timeout = Mathf.Infinity) {
        throw new NotImplementedException();
    }

    protected void Shoot(Projectile2D projectile) => Shoot(look.Direction, projectile);

    protected void Shoot(Vector2 direction, Projectile2D projectile) {
        projectile.Launch(transform.position, direction);
    }

    protected void Swing(Collider2D hitboxShape, DamageInfo damageInfo) => Swing(hitboxShape, look.Direction, damageInfo);

    protected void Swing(Collider2D hitboxShape, Vector2 direction, DamageInfo damageInfo) {
        float angle = Vector2.Angle(Vector2.up, direction);
        // Someone please make this cleaner - S.
        List<Collider2D> results = new();
        int n = hitboxShape.Overlap(transform.position, angle, results);
        for (int i = 0; i < results.Count; i++) {
            results[i].BroadcastMessage("OnHitBySwing", damageInfo, SendMessageOptions.DontRequireReceiver);
        }
    }

    protected IdleState IdleFor(float seconds, string animation) {
        return IdleState.ForSeconds(seconds, animation);
    }

    /// <summary>
    /// Kicks back to the previous state.
    /// </summary>
    protected void KickBack() {
        if(hasPrev) {
            Then(prev);
        } else {
            throw new InvalidOperationException("Cannot kick back from initial state: there is no state to return to.");
        }
    }

    public void PlayAnimation(string name) {
        if(animator != null) {
            animator.PlayInFixedTime(name);
        }
    }
}

}
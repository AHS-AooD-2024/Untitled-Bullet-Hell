using UnityEngine;
using Entities.States;
using Combat;

public class ZombieStateMachine : StateMachine
{

    [SerializeField]
    private DamageInfo m_contactDamage;

    protected override EntityState GetInitialState()
    {
        return new SearchState(10.0f, new DynamicWaypointMoveState(player.transform));
    }

    protected override EntityState GetDeathState() {
        return new DeathState();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        other.collider.BroadcastMessage("OnHitBySwing", m_contactDamage, SendMessageOptions.DontRequireReceiver);
    }
}

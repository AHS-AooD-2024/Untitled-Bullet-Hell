using UnityEngine;

namespace Entities.States {
public class StaticWaypointMoveState : EntityState {
    private readonly float m_timeout;

    private readonly Vector2 m_waypoint;

    public StaticWaypointMoveState(Vector2 position, float timeout) {
        m_waypoint = position;
        m_timeout = timeout;
    }

    protected override void OnExit() {
    }

    protected override void OnUpdate() {
        if(age > m_timeout) {
            KickBack();
        }

        if(IsCloseEnough()) {
            KickBack();
        } else {
            // TODO: nav mesh or something
            Vector2 pq = m_waypoint - (Vector2) transform.position;
            pq.Normalize();
            controller.Move(pq, false, deltaTime);
        }
    }

    private bool IsCloseEnough() {
        float dist = Vector2.Distance(transform.position, m_waypoint);
        return dist <= Vector2.kEpsilon;
    }

    protected override void OnEnter() {
    }
}

}
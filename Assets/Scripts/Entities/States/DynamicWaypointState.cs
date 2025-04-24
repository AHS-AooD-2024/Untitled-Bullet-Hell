using UnityEngine;

namespace Entities.States {
public class DynamicWaypointMoveState : EntityState {
    private readonly float m_timeout;

    private readonly Transform m_waypoint;

    public DynamicWaypointMoveState(Transform follow, float timeout = Mathf.Infinity) {
        m_waypoint = follow;
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
            Vector2 pq = m_waypoint.position - transform.position;
            pq.Normalize();
            controller.Move(pq, false, deltaTime);
        }
    }

    private bool IsCloseEnough() {
        float dist = Vector2.Distance(transform.position, m_waypoint.position);
        return dist <= Vector2.kEpsilon;
    }

    protected override void OnEnter() {
    }
}

}
using UnityEngine;
using Util;

namespace Entities.States {
    
public class SearchState : EntityState {
    private readonly ContactFilter2D m_filter;

    private readonly float m_sightRange;

    private readonly EntityState m_after;

    public SearchState(ContactFilter2D filter, float sightRange, EntityState after) {
        m_filter = filter;
        m_sightRange = sightRange;
        m_after = after;
    }
    
    public SearchState(float sightRange, EntityState after) {
        m_filter = new ContactFilter2D().NoFilter();
        m_sightRange = sightRange;
        m_after = after;
    }

    protected override void OnEnter() {
    }

    protected override void OnExit() {
    }

    protected override void OnUpdate() {
        LineOfSight los = LineOfSight.Check(collider, player, m_filter);
        Debug.Log(los.HasLineOfSight);
        if(los.HasLineOfSight && los.Hit.distance < m_sightRange) {
            Then(m_after);
        }
    }
}

}
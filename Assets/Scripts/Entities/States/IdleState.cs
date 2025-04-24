using UnityEngine;

namespace Entities.States {
public class IdleState : EntityState {
    private readonly float m_timeout;

    private readonly string m_animationName;

    private IdleState(float timeout, string animationName) {
        m_timeout = timeout;
        m_animationName = animationName;
    }

    public IdleState(string animationName) {
        m_timeout = Mathf.Infinity;
        m_animationName = animationName;
    }

    public static IdleState ForSeconds(float seconds, string animationName) {
        return new IdleState(seconds, animationName);
    }

    protected override void OnExit() {
    }

    protected override void OnUpdate() {
        if(age > m_timeout) {
            KickBack();
        }
    }

    protected override void OnEnter() {
        PlayAnimation(m_animationName);
    }
}

}
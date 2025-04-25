using Entities.States;
using UnityEngine;
using Util;

namespace Entities {
    public class EyeCircleState : EntityState {
        private readonly float m_shootDelay;
        private readonly float m_distanceFromPlayer;
        private readonly float m_period;
        private readonly bool m_doMoveClockwise; 

        private readonly SitAndShoot m_shootState;

        private float m_targetTheta;

        public EyeCircleState(float shootInterval, float distanceFromPlayer, float period, Projectile2D projectile, bool doMoveClockwise = false) {
            m_shootDelay = shootInterval;
            m_distanceFromPlayer = distanceFromPlayer;
            m_shootState = new SitAndShoot(3, 1.0F, projectile, 8.0F, true);
            m_doMoveClockwise = doMoveClockwise;
            m_period = period;
        }

        protected override void OnEnter() {
            Vector2 directionFromPlayer = transform.position - player.transform.position;
                
            m_targetTheta = Vector2.SignedAngle(Vector2.right, directionFromPlayer) * Mathf.Deg2Rad;
        }

        protected override void OnExit() {
        }

        protected override void OnUpdate() {
            Debug.DrawRay(transform.position, look.Direction * 100, Color.red);

            if(age > m_shootDelay) {
                Then(m_shootState);
            } else {
                Vector2 directionToPlayer = player.transform.position - transform.position;

                Debug.DrawLine(player.transform.position, transform.position);
                
                float deltaTheta = 2.0F * Mathf.PI / m_period * (m_doMoveClockwise ? -1.0F : 1.0F) * deltaTime;
                m_targetTheta += deltaTheta;

                float x = m_distanceFromPlayer * Mathf.Cos(m_targetTheta);
                float y = m_distanceFromPlayer * Mathf.Sin(m_targetTheta);

                Vector2 goal = (Vector2) player.transform.position + new Vector2(x, y);

                // FIXME: The thing will bob about its goal position, which kinda sucks.
                // I think it could be fixed with a PID controller, smoothing the acceleration
                // makes it look not terrible, so this is low priority
                if(IsCloseEnough(goal)) {
                    controller.Move(Vector2.zero, directionToPlayer, false, deltaTime);
                } else {
                    #if UNITY_EDITOR
                    Gizmos2D.DrawCircle(goal, 0.5F);

                    Debug.DrawLine(goal, transform.position);
                    #endif

                    Vector2 directionToGoal = goal - (Vector2) transform.position;
                    directionToGoal.Normalize();
                    controller.Move(directionToGoal, directionToPlayer, false, deltaTime);
                }
            }
        }

        private bool IsCloseEnough(in Vector2 goal) {
            return Vector2.Distance(transform.position, goal) < 0.1F;
        }
    }
}
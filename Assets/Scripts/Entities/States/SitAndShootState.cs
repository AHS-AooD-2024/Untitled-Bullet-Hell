using Entities.States;
using UnityEngine;

namespace Entities.States {
    public class SitAndShoot : EntityState {
        private readonly float m_lifespan;

        private readonly Projectile2D m_projectile;

        private readonly float m_shotTimeInterval;
        private float m_nextShotTime;
        private readonly bool m_doStare;

        private readonly float m_devianceRads;

        public SitAndShoot(int numShots, float lifespan, Projectile2D projectile, float deviance = 0.0F, bool doStare = false) {
            m_lifespan = lifespan;

            m_projectile = projectile;

            m_shotTimeInterval = lifespan / numShots;
            m_doStare = doStare;
            m_devianceRads = deviance * Mathf.Deg2Rad;
        }

        protected override void OnEnter() {
            m_nextShotTime = 0.0F;
            controller.Move(Vector2.zero, false, deltaTime);
        }

        protected override void OnExit() {
        }

        protected override void OnUpdate() {

            if(m_doStare) {
                controller.Move(Vector2.zero, player.transform.position - transform.position, false, deltaTime);
            } else {
                controller.Move(Vector2.zero, false, deltaTime);
            }
            

            if(age >= m_lifespan) {
                KickBack();
            } else if(age > m_nextShotTime) {
                if(m_devianceRads == 0.0F) {
                    Shoot(m_projectile); // no calls to random
                } else {
                    float thetaOffset = Random.Range(-m_devianceRads, m_devianceRads);
                    float theta = Mathf.Deg2Rad * Vector2.SignedAngle(Vector2.right, look.Direction) + thetaOffset;
                    Vector2 dir = new(Mathf.Cos(theta), Mathf.Sin(theta));
                    Debug.DrawRay(transform.position, dir * 100, Color.green, m_shotTimeInterval);
                    Debug.DrawRay(transform.position, look.Direction * 100, Color.red, m_shotTimeInterval);
                    Shoot(dir, m_projectile);
                }
                m_nextShotTime += m_shotTimeInterval;
            }
        }
    }
}
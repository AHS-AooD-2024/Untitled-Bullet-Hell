using System;
using System.Collections.Generic;
using Nevelson.Topdown2DPitfall.Assets.Scripts.Utils;
using UnityEngine;

namespace Util {
    public class LineOfSight : IEquatable<LineOfSight> {
        public RaycastHit2D Hit { get; private set; }
        public bool HasLineOfSight { get; private set; }

        private LineOfSight(RaycastHit2D hit, bool los) {
            Hit = hit;
            HasLineOfSight = los;
        }

        public override bool Equals(object obj) {
            return obj is LineOfSight check && Equals(check);
        }

        public bool Equals(LineOfSight other) {
            return Hit == other.Hit && other.HasLineOfSight == other.HasLineOfSight;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Hit, HasLineOfSight);
        }

        public static bool operator==(LineOfSight left, LineOfSight right) {
            return left.Equals(right);
        }

        public static bool operator !=(LineOfSight left, LineOfSight right) {
            return !(left == right);
        }

        public static implicit operator bool(LineOfSight los) {
            return los.HasLineOfSight;
        }
        

        private static RaycastHit2D[] m_raycastCache;
        private static Collider2D[] m_colliderCache;

        private static ContactFilter2D m_defaultFilter = DefaultFilter();

        private static ContactFilter2D DefaultFilter() {
            ContactFilter2D filter = new();
            filter.SetLayerMask(
                ~(LayerMask.NameToLayer(Constants.PITFALL_COLLIDER) | LayerMask.NameToLayer("Ignore Raycast"))
            );

            return filter;
        }

        private static void EnsureRaycastCache() {
            m_raycastCache ??= new RaycastHit2D[8];
        }

        private static void EnsureColliderCache() {
            m_colliderCache ??= new Collider2D[8];
        }

        public static LineOfSight Check(Collider2D eye, Collider2D target) {
            return Check(eye, target, m_defaultFilter);
        }
        
        /// <summary>
        /// Checks the line of sight between two colliders and returns the distance 
        /// between their center, or <c>-1.0F</c> if the colliders do not have line 
        /// of sight
        /// </summary>
        /// <param name="eye">The collider from.</param>
        /// <param name="target">The collider to.</param>
        /// <param name="filter">A filter for things to ignore when checking line of sight.</param>
        /// <returns>A <see cref="LineOfSightCheck"/></returns>
        public static LineOfSight Check(Collider2D eye, Collider2D target, ContactFilter2D filter) {
            EnsureRaycastCache();
            lock (m_raycastCache) {
                
                // Debug.DrawLine(eye.transform.position, target.transform.position, Color.blue);
                int n = Physics2D.Linecast(eye.bounds.center, target.bounds.center, filter, m_raycastCache);
                Debug.DrawLine(eye.bounds.center, target.bounds.center, Color.green);

                for(int i = 0; i < n; i++) {
                    RaycastHit2D hit = m_raycastCache[i];
                    Debug.Log(hit.collider);
                    if(hit.collider == target) {
                        return new LineOfSight(hit, true);
                    } else if(hit.collider != eye) {
                        return new LineOfSight(hit, false);
                    }
                }

                throw new ArgumentException("Line of sight check failed: target " + target + " was not found by linecast.");
            }
        }
    }
}
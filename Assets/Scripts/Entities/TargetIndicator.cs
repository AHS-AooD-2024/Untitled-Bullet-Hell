using Combat;
using UnityEngine;

namespace Entities {
    
    public class TargetIndicator : MonoBehaviour {

        [HideInInspector]
        public ProjectileInstance2D corrospondingProjectile;

        public void OnHitByProjectile(ProjectileInstance2D proj) {
            if(ReferenceEquals(corrospondingProjectile, proj)) {
                Destroy(gameObject);
                proj.Consume();
            }
        }
    }

}
using System;
using Combat;
using UnityEngine;

namespace Util {
    
public class StickProjectiles : MonoBehaviour {
    [SerializeField]
    private Alliance m_interactWith = Alliance.NONE;

    public void OnHitByProjectile(ProjectileInstance2D proj) {
        if((m_interactWith & proj.prototype.alliance) != 0) {
            proj.StickTo(this);
        }
    }
}

}
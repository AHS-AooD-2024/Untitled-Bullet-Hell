using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuchTest : MonoBehaviour {
    public void OnHitBySwing() {
        Debug.Log("Ouch!");
    }

    public void OnHitByProjectile(ProjectileInstance2D proj) {
        if(proj.prototype.alliance == Projectile2D.Alliance.PLAYER) {
            Debug.Log("Oof!");
            proj.Consume();
        }
    }
}

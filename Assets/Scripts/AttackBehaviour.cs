using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour {

    public void Swing(float range, float breadth) {
        Vector2 fwd = transform.TransformDirection(Vector3.up);
        Vector2 pos2D = transform.position;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(pos2D, breadth * 0.5F, fwd, range);
        foreach (RaycastHit2D hit in hits) {
            if(hit) {
                hit.transform.BroadcastMessage("OnHitBySwing", SendMessageOptions.DontRequireReceiver);
                Debug.DrawLine(pos2D, pos2D + range * fwd, Color.white, 1.0F);
                Debug.DrawLine(pos2D - Vector2.Perpendicular(fwd) * breadth, pos2D + Vector2.Perpendicular(fwd) * breadth, Color.white, 1.0F);
            }
        }
    }
}

using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private float deadEntities = 0;
    [SerializeField] private List<GameObject> entities;

    public void UpdateDeadCounter() {
        deadEntities++;
        if (deadEntities == entities.Count) {
            Destroy(gameObject);
        }
    }
}

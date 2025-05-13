using UnityEngine;
namespace Combat {
    
public class DestructableCover : MonoBehaviour {
    public void OnDeath() {
        Destroy(gameObject);
    }
}
}
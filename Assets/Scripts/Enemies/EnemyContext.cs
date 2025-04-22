using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(EnemyDetection), typeof(EnemyBehaviorManager))]
public class EnemyContext : MonoBehaviour
{
    public EnemyHealth Health {get; set;}
    public EnemyDetection Detection {get; set;}
    public EnemyBehaviorManager BehaviorManager {get; set;}

    protected virtual void Start()
    {
        Health = GetComponent<EnemyHealth>();
        Detection = GetComponent<EnemyDetection>();
        BehaviorManager = GetComponent<EnemyBehaviorManager>();
    } 
}
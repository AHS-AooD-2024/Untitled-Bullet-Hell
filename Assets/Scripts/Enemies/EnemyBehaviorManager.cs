using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(EnemyDetection))]
public abstract class EnemyBehaviorManager : MonoBehaviour {
    protected EnemyHealth health;
    protected EnemyDetection detection;
    protected BehaviorTree behaviorTree;

    protected void Start()
    {
        health = GetComponent<EnemyHealth>();
        detection = GetComponent<EnemyDetection>();
        CreateTree();
    }

    public abstract void CreateTree();

    public EnemyHealth GetHealth()
    {
        return health;
    }
}
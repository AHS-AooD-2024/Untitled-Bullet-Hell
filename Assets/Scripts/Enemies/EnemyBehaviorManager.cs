using UnityEngine;

[RequireComponent(typeof(EnemyHealth), typeof(EnemyDetection))]
public abstract class EnemyBehaviorManager : MonoBehaviour {
    protected EnemyHealth health;
    //protected EnemyDetection detection;
    protected BehaviorTree behaviorTree;

    protected virtual void Start()
    {
        //health = GetComponent<EnemyHealth>();
        //detection = GetComponent<EnemyDetection>();
        behaviorTree = CreateTree();
        behaviorTree.Start();
    }

    public abstract BehaviorTree CreateTree();
}
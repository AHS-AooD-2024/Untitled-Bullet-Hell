using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BehaviorObject", menuName = "Scriptable Objects/BehaviorObject")]
public class BehaviorObject : ScriptableObject
{
    [field: SerializeField] public float Delay { get; set; } = 1f;
    [field: SerializeField] public bool RepeatIfEnd {get; set;}
    [SerializeField] protected IBehavior behavior;
    [SerializeField] protected List<BehaviorObject> nextBehaviors;

    public List<IBehavior> GetNextBehaviorsAsList() => nextBehaviors;
    public IEnumerator DoBehavior() => behavior.DoBehavior();
    public bool BehaviorCondition() => behavior.BehaviorCondition();
}

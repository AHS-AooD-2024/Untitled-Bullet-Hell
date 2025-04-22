using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BehaviorObject", menuName = "BehaviorObject")]
public abstract class BehaviorObject : ScriptableObject
{
    [field: SerializeField] public float Delay { get; set; } = 1f;
    [field: SerializeField] public bool RepeatIfEnd {get; set;} = false;
    [SerializeField] protected List<BehaviorObject> nextBehaviors;

    public List<BehaviorObject> GetNextBehaviorsAsList() => nextBehaviors;
    public abstract IEnumerator DoBehavior();
    public abstract bool BehaviorCondition();
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class BehaviorObject : ScriptableObject
{
    [field: SerializeField] public float Delay { get; set; } = 1f;
    [field: SerializeField] public bool RepeatIfEnd {get; set;} = false;
    [SerializeField] protected List<BehaviorObject> nextBehaviors;

    public List<BehaviorObject> GetNextBehaviorsAsList() => nextBehaviors;
    public abstract IEnumerator DoBehavior<T>(T executor) where T : IExecutor;
    public abstract bool BehaviorCondition<T>(T executor) where T : IExecutor;
}

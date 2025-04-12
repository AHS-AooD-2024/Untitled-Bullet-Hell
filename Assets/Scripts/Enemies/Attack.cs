using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : IBehavior
{
    public float Delay { get; set; } = 1f;
    public bool RepeatIfEnd { get; set; } = false;

    private Func<bool> conditionCheck;
    private Func<IEnumerator> performAttack;
    private List<IBehavior> nextBehaviors;

    public Attack(Func<bool> conditionCheck, Func<IEnumerator> performAttack)
    {
        this.conditionCheck = conditionCheck;
        this.performAttack = performAttack;
        nextBehaviors = new();
    }


    public bool BehaviorCondition() => conditionCheck.Invoke();
    public IEnumerator DoBehavior() => performAttack.Invoke();
    public void AddNextBehavior(IBehavior node) => nextBehaviors.Add(node);
    public void AddNextBehaviors(List<IBehavior> nodes) => nextBehaviors.AddRange(nodes);
    public List<IBehavior> GetNextBehaviorsAsList() => nextBehaviors;
}
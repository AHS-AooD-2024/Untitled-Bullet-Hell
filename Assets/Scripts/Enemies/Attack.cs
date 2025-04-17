using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : IBehavior
{
    private Func<bool> conditionCheck;
    private Func<IEnumerator> performAttack;

    public Attack(Func<bool> conditionCheck, Func<IEnumerator> performAttack)
    {
        this.conditionCheck = conditionCheck;
        this.performAttack = performAttack;
    }


    public bool BehaviorCondition() => conditionCheck.Invoke();
    public IEnumerator DoBehavior() => performAttack.Invoke();
}
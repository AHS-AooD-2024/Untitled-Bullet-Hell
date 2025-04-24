using System.Collections.Generic;
using UnityEngine;

public class FirstBossBehaviorManager : EnemyBehaviorManager
{    
    [Header("ATTACK EXECUTOR HERE")]
    [SerializeField] BossAttackExecutor executor;
    public override BehaviorTree CreateTree()
    {
        Attack attack = new(executor.SweepCondition, executor.PerformSweep)
        {
            RepeatIfEnd = true
        };
        BehaviorTree tree = new(attack, this);
        return tree;
    }
}

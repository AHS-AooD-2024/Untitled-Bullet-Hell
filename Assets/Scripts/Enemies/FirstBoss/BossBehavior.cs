using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "BossBehaviorObject", menuName = "BossBehaviorObject")]
public class BossBehavior : BehaviorObject
{
    [SerializeField] protected BossAttackExecutor executor;
    public override bool BehaviorCondition()
    {
        return executor.SweepCondition();
    }

    public override IEnumerator DoBehavior()
    {
        yield return executor.PerformSweep();
    }
}
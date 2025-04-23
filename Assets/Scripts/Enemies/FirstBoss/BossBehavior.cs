using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "BossBehaviorObject", menuName = "BossBehaviorObject")]
public class BossBehavior : BehaviorObject
{
    public override bool BehaviorCondition<BossAttackExecutor>(BossAttackExecutor executor)
    {
        executor.Swee
    }

    public override IEnumerator DoBehavior<BossAttackExecutor>(BossAttackExecutor executor)
    {
        throw new System.NotImplementedException();
    }
}
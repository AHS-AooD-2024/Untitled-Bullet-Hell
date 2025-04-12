
//for refactoring in case...
using System.Collections;
using System.Collections.Generic;

public interface IBehavior
{
    public bool RepeatIfEnd {get; set;}
    public float Delay {get; set;}

    public IEnumerator DoBehavior();
    public void AddNextBehavior(IBehavior node);
    public void AddNextBehaviors(List<IBehavior> nodes);
    public List<IBehavior> GetNextBehaviorsAsList();
    public bool BehaviorCondition();
}
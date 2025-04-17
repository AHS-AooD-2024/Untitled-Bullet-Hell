
//for refactoring in case...
using System.Collections;
using System.Collections.Generic;

public interface IBehavior
{
    public IEnumerator DoBehavior();
    public bool BehaviorCondition();
}
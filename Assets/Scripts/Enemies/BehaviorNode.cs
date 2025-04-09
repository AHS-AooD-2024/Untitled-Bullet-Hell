using System;

//represents a single behavior
//behaviors are ienumerators probably
public abstract class BehaviorNode
{
    //maybe contains previous attack as info? idk
    private event Action behavior;
    private List<BehaviorNode> nextBehaviors;
    public float Duration {get; set;} = 1f;

    //Used if an behavior should occur more often than another neighboring behavior
    //public float Weight {get; set;} = 1f;
    
    //starts the behavior event 
    //(the enemy subscribes with their listener to do attack)
    public void Broadcast()
    {
        behavior?.Invoke();
    }
    public void Run()
    {
        Broadcast();
    }
    //The condition needed for this behavior to run
    //ex) melee should be close to player (who is probs a singleton)
    public abstract boolean BehaviorCondition();
}
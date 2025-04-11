using System;
using System.Collections.Generic;

//represents a single behavior
//behaviors are ienumerators probably
public abstract class BehaviorNode
{
    //maybe contains previous attack as info? idk
    private event Action behavior;
    private List<BehaviorNode> nextBehaviors;

    //if no next behaviors, either repeats, or goes to root node
    public bool RepeatIfEnd {get; set;} = false;
    public float Duration {get; set;}
    public float Delay {get; set;}

    public BehaviorNode(float duration, float delay, Action behavior)
    {
        Duration = duration;
        Delay = delay;
        this.behavior += behavior;
        nextBehaviors = new();
    }
    public BehaviorNode(Action behavior) : this(1f, 1f, behavior)
    {
    }
    
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
    public void AddNextBehavior(BehaviorNode node)
    {
        nextBehaviors.Add(node);
    }
    public void AddNextBehaviors(List<BehaviorNode> nodes)
    {
        nextBehaviors.AddRange(nodes);
    }
    public List<BehaviorNode> GetNextBehaviorsAsList()
    {
        return nextBehaviors;
    }
    //The condition needed for this behavior to run
    //ex) melee should be close to player (who is probs a singleton)
    //should maybe 
    public abstract bool BehaviorCondition();
}
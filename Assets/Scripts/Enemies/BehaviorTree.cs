using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents the start of a behavior tree (instead of holding the head node)
public class BehaviorTree
{
    private BehaviorNode root;
    private BehaviorNode current;
    private EnemyBehaviorManager manager;
    private bool isAlive;
    private bool detectsPlayer;

    public BehaviorTree(BehaviorNode root, EnemyBehaviorManager manager)
    {
        this.root = root;
        current = null;
        this.manager = manager;
        isAlive = true;
        detectsPlayer = false;
        manager.GetHealth().SubscribeToDeathEvent(RespondToDeath);
    }

    private void RespondToDeath(EnemyHealth e)
    {
        isAlive = false;
    }
    /// <summary>
    /// Begins the Running the Behavior Tree
    /// </summary>
    public void Start()
    {
        current = root;
        detectsPlayer = true;
        while (true)
        {
            manager.StartCoroutine(StartCurrentBehavior());
            current = FindNextBehavior();
            if (!isAlive || !detectsPlayer) return;
        }
    }
    private IEnumerator StartCurrentBehavior()
    {
        yield return new WaitForSeconds(current.Delay);
        current.Run();
        yield return new WaitForSeconds(current.Duration);
    }
    private BehaviorNode FindNextBehavior()
    {
        List<BehaviorNode> nextBehaviors = current.GetNextBehaviorsAsList();
        if (nextBehaviors.Count == 0)
        {
            if (!current.RepeatIfEnd)
            {
                return root;
            }
            else
                return current;
        }
        //TODO: add weighting, ie more likely for close moves when close, etc.
        List<BehaviorNode> validBehaviors = new();
        foreach(BehaviorNode behavior in nextBehaviors)
        {
            if (behavior.BehaviorCondition()) validBehaviors.Add(behavior);
        }
        if (validBehaviors.Count == 0)
        {
            //no valid behaviors, uhhhh
            return current;
        }

        return validBehaviors[(int)UnityEngine.Random.Range(0f, validBehaviors.Count)];
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents the start of a behavior tree (instead of holding the head node)
public class BehaviorTree
{
    private IBehavior root;
    private IBehavior current;
    private EnemyBehaviorManager manager;
    private bool isAlive;
    private bool detectsPlayer;

    public BehaviorTree(IBehavior root, EnemyBehaviorManager manager)
    {
        this.root = root;
        current = null;
        this.manager = manager;
        isAlive = true;
        detectsPlayer = false;
        //manager.GetHealth().AddOnDeathEventListener(RespondToDeath);
    }

    private void RespondToDeath(EnemyHealth e)
    {
        isAlive = false;
    }
    public void DetectsPlayer(bool detects)
    {
        detectsPlayer = detects;
    }
    /// <summary>
    /// Begins the Running the Behavior Tree
    /// </summary>
    public void Start()
    {
        current = root;
        detectsPlayer = true;
        manager.StartCoroutine(RunBehaviorTree());
    }

    private IEnumerator RunBehaviorTree()
    {
        while (isAlive && detectsPlayer)
        {
            yield return manager.StartCoroutine(StartCurrentBehavior());
            current = FindNextBehavior();
        }
    }
    private IEnumerator StartCurrentBehavior()
    {
        Debug.Log("ATTACK DELAY: " + current.Delay);
        yield return new WaitForSeconds(current.Delay);
        Debug.Log("ATTACKING");
        yield return manager.StartCoroutine(current.DoBehavior());
    }
    private IBehavior FindNextBehavior()
    {
        List<IBehavior> nextBehaviors = current.GetNextBehaviorsAsList();
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
        List<IBehavior> validBehaviors = new();
        foreach(IBehavior behavior in nextBehaviors)
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
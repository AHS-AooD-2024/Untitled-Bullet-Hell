using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents the start of a behavior tree (instead of holding the head node)
public class BehaviorTree
{
    private BehaviorObject root;
    private BehaviorObject current;
    private EnemyContext context;
    private bool isActive = false;

    public BehaviorTree(BehaviorObject root, EnemyContext ctx)
    {
        this.root = root;
        current = null;
        context = ctx;
    }

    public void SetActive(bool isActive) => this.isActive = isActive;

    /// <summary>
    /// Begins the Running the Behavior Tree
    /// </summary>
    [ContextMenu("RUN TREE")]
    public void Start()
    {
        current = root;
        isActive = true;
        context.StartCoroutine(RunBehaviorTree());
    }

    private IEnumerator RunBehaviorTree()
    {
        while (isActive)
        {
            yield return context.StartCoroutine(StartCurrentBehavior());
            current = FindNextBehavior();
        }
    }
    private IEnumerator StartCurrentBehavior()
    {
        yield return new WaitForSeconds(current.Delay);
        yield return context.StartCoroutine(current.DoBehavior());
    }
    private BehaviorObject FindNextBehavior()
    {
        List<BehaviorObject> nextBehaviors = current.GetNextBehaviorsAsList();
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
        List<BehaviorObject> validBehaviors = new();
        foreach(BehaviorObject behavior in nextBehaviors)
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
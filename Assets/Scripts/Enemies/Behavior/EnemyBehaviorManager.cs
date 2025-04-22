
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the tree
/// </summary>
[RequireComponent(typeof(EnemyContext))]
public class EnemyBehaviorManager : MonoBehaviour {
    [SerializeField] protected Dictionary<EnemyState, BehaviorTree> behaviorTrees = new();
    protected EnemyContext context;

    protected virtual void Start()
    {
        context = GetComponent<EnemyContext>();
    }

    [SerializeField] protected BehaviorObject root;
    public virtual BehaviorTree CreateTree()
    {
        return new(root, context);
    }
}
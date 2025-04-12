using System;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
    //2 modes: idle, & attack for now ig

    [SerializeField] private EnemyState currentState = EnemyState.IDLE;

    private event Action<EnemyState> onStateChanged;

    public void AddStateEventListener(Action<EnemyState> actionListener)
    {
        onStateChanged += actionListener;
    }
    public void RemoveStateEventListener(Action<EnemyState> actionListener)
    {
        onStateChanged -= actionListener;
    }
    protected void ChangeState(EnemyState state)
    {
        if (currentState != state)
        {
            currentState = state;
            onStateChanged?.Invoke(currentState);
        }
    }
    protected void Update()
    {
        //TEMPORARY
        if (currentState == EnemyState.IDLE) ChangeState(EnemyState.ATTACK);
    }
}
public enum EnemyState 
{
    IDLE,
    ATTACK,
}
using System;
using System.Collections;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
    //2 modes: idle, & attack for now ig

    [SerializeField] private EnemyState currentState = EnemyState.IDLE;
    [SerializeField] protected float interval = 0.1f;
    [SerializeField] protected float detectionRadius = 1f;

    public event Action<EnemyState> onStateChanged;
    protected void ChangeState(EnemyState state)
    {
        if (currentState != state)
        {
            currentState = state;
            onStateChanged?.Invoke(currentState);
        }
    }
    
    protected void Start()
    {
        StartCoroutine(StartDetection());
    }
    private IEnumerator StartDetection()
    {
        while (true)
        {
            DetectPlayer();
            yield return new WaitForSeconds(interval);
        }
    }
    //TEMPORARY
    [SerializeField] Transform playerTransform;
    protected virtual void DetectPlayer()
    {
        bool seenPlayer = false;
        RaycastHit2D[] hits = Physics2D.CircleCastAll((Vector2)transform.position, detectionRadius, Vector2.up);
        foreach (var hit in hits)
        {
            if (hit.collider.transform == playerTransform) seenPlayer = true;
        }
        if (seenPlayer) ChangeState(EnemyState.ATTACK);
        else ChangeState(EnemyState.IDLE);
    }
    public EnemyState GetState() => currentState;
}
public enum EnemyState 
{
    IDLE,
    ATTACK,
}
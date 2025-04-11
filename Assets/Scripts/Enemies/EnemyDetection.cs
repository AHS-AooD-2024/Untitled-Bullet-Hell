using System;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {
    //2 modes: idle, & attack for now ig
    

    [SerializeField] private EnemyState currentState = EnemyState.IDLE;

    public event Action<EnemyState> onStateChanged;

}
public enum EnemyState 
{
    IDLE,
    ATTACK,
}
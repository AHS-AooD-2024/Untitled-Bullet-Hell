using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //do health stuff
    private event Action<EnemyHealth> onDeath;
    public void AddOnDeathEventListener(Action<EnemyHealth> e)
    {
        onDeath += e;
    }
    public void RemoveOnDeathEventListener(Action<EnemyHealth> e)
    {
        onDeath -= e;
    }
    public virtual void Die()
    {
        onDeath?.Invoke(this);
    }
}
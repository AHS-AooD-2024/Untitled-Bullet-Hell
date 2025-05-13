using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health;
    public float Health 
    {
        get => health;
        set
        {
            if (health != value)
            {
                HpChanged();
            }
            health = value;
        }
    }
    //do health stuff
    public event Action<EnemyHealth> OnDeath;
    public event Action<EnemyHealth> OnHpChange;
    protected virtual void Start()
    {
        //Broadcast death when health below 0 when hp changes
        OnHpChange += (health) => {if (health.Health <= 0) Die();};
    }    
    protected virtual void HpChanged()
    {
        OnHpChange?.Invoke(this);
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
    }
}
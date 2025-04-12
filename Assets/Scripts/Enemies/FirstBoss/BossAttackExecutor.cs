using System.Collections;
using UnityEngine;

public class BossAttackExecutor : MonoBehaviour
{
    [SerializeField] private Transform sweepPoint;
    [SerializeField] private GameObject sweepAttack;

    public IEnumerator PerformSweep()
    {
        Debug.Log("SWEEEEEEEEEEEEEPING");
        GameObject sweep = Instantiate(sweepAttack, sweepPoint);
        yield return new WaitForSeconds(2f);
        Destroy(sweep);
    }
    public bool SweepCondition() => true;
}
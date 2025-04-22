using System.Collections;
using System.IO;
using UnityEngine;

public class BossAttackExecutor : MonoBehaviour
{
    [SerializeField] private Transform sweepPoint;
    [SerializeField] private Transform sweepOrigin;
    [SerializeField] private GameObject sweepAttack;
    [SerializeField] private Transform player;

    public IEnumerator PerformSweep()
    {
        Debug.Log("SWEEEEEEEEEEEEEPING");

        Vector2 direction = player.position - sweepOrigin.position;
        //what tf is this sht lmao thank god for discussion posts
        sweepOrigin.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        GameObject sweep = Instantiate(sweepAttack, sweepPoint);
        yield return new WaitForSeconds(2f);
        Destroy(sweep);
    }
    public bool SweepCondition() => true;
}
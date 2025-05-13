using System.Threading;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    [SerializeField] GameObject fullHeart;
    [SerializeField] GameObject emptyHeart;

    void Start()
    {
        fullHeart.SetActive(true);
        emptyHeart.SetActive(false);
    }

    public void SetFull(bool full)
    {
        fullHeart.SetActive(full);
        emptyHeart.SetActive(!full);
    }
}

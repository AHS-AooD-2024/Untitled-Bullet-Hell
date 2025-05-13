using Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FountainScript : Interactable {
    [SerializeField] private GameObject player;
    private PlayerHealth playerHealth;

    protected override void Start()
    {
        base.Start();
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E)) {
            ResetHealth();
        } 
    }

    void ResetHealth() {
       playerHealth.ResetHealth();
    }
}
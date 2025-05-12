using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private Color32 interactableColor;
    private Color32 defaultColor;
    [SerializeField] private SpriteRenderer sprite;
    protected bool inRange;

    protected virtual void Start()
    {
        defaultColor = new Color32(255, 255, 255, 255);
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            sprite.color = interactableColor;
            inRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            sprite.color = defaultColor;
            inRange = false;
        }
    }
}

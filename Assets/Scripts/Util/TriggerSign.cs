using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TriggerSign : MonoBehaviour {
    #if UNITY_EDITOR
    [MenuItem("GameObject/UI/World Canvas", false, 10)]
    private static void CreateWorldCanvas(MenuCommand menuCommand) {
        GameObject gameObject = new("Canvas");
        GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
        GameObjectUtility.EnsureUniqueNameForSibling(gameObject);
        Canvas canvas = gameObject.AddComponent<Canvas>();
        _ = gameObject.AddComponent<CanvasScaler>();

        canvas.renderMode = RenderMode.WorldSpace;

        Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
        Selection.activeGameObject = gameObject;
    }
    #endif

    [SerializeField]
    private TextMeshProUGUI m_tmp;

    [SerializeField]
    private float m_fadeInDurationSeconds = 1.0F;

    [SerializeField]
    private float m_fadeOutDurationSeconds = 1.0F;

    [SerializeField]
    private string m_triggererTag = "Player";

    /// <summary>
    /// The number of things that are causing this sign to reveal its text.
    /// This is so that if multiple things can reveal the sign text, one leaving
    /// while the other stays will not prematurely hide the text.
    /// </summary>
    private int m_numThingsRevealing = 0;

    private void Awake() {
        if(m_tmp == null) {
            m_tmp = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        HideText();
    }

    private void ShowText() {
        Debug.Log("Showing");
        m_tmp.CrossFadeAlpha(1.0F, m_fadeInDurationSeconds, false);
    }

    private void HideText() {
        Debug.Log("Hiding");
        m_tmp.CrossFadeAlpha(0.0F, m_fadeInDurationSeconds, false);
    }

    private bool CanTrigger(Collider2D collider) => collider.CompareTag(m_triggererTag);

    private void OnTriggerEnter2D(Collider2D other) {
        // short circuit will keep the num tracker accurte
        if (CanTrigger(other) && ++m_numThingsRevealing > 0) {
            ShowText();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (CanTrigger(other) && --m_numThingsRevealing <= 0) {
            HideText();
        }
    }
}

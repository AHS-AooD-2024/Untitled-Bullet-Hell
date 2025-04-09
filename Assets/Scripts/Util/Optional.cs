using UnityEngine;
#if UNITY_EDITOR
  using UnityEditor;
#endif

namespace Util {

// From https://stackoverflow.com/questions/52854129/unity-doesnt-serialize-int-field

/// <summary>
/// Does the same as C# System.Nullable, except it's an ordinary
/// serializable struct, allowing unity to serialize it and show it in the inspector.
/// </summary>
[System.Serializable]
public struct Optional<T> where T : struct {
    public T Value { get {
        if (!HasValue)
            throw new System.InvalidOperationException("Serializable nullable object must have a value.");
        return m_value;
    } }

    public bool HasValue { get { return m_hasValue; } }

    [SerializeField]
    private T m_value;

    [SerializeField]
    private bool m_hasValue;

    public static Optional<T> Empty { get => new Optional<T>(); }

    public static Optional<T> Of(T value) {
        return new Optional<T>(value);
    }
    
    public static Optional<T> OfNullable(T? value) {
        if(value == null) {
            return Empty;
        } else {
            return new Optional<T>((T) value);
        }
    }

    private Optional(bool hasValue, T v) {
        this.m_value = v;
        this.m_hasValue = hasValue;
    }

    private Optional(T v) {
        this.m_value = v;
        this.m_hasValue = true;
    }

    // private Optional() {
    //     this.m_hasValue = false;
    //     this.m_value
    // }

    public static implicit operator Optional<T>(T value) {
        return new Optional<T>(value);
    }

    public static implicit operator Optional<T>(System.Nullable<T> value) {
        return value.HasValue ? new Optional<T>(value.Value) : new Optional<T>();
    }

    public static implicit operator System.Nullable<T>(Optional<T> value) {
        return value.HasValue ? (T?)value.Value : null;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Optional<>))]
internal class OptionalDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUILayout.BeginHorizontal();

        // Draw label
        EditorGUILayout.PrefixLabel(label);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        var hasValueProp = property.FindPropertyRelative("m_hasValue");
        EditorGUILayout.PropertyField(hasValueProp, GUIContent.none);

        // Have the value field on the next line
        EditorGUILayout.EndHorizontal();
        
        bool guiEnabled = GUI.enabled;
        GUI.enabled = guiEnabled && hasValueProp.boolValue;
        EditorGUILayout.PropertyField(property.FindPropertyRelative("m_value"), GUIContent.none);
        GUI.enabled = guiEnabled;
    }
}
#endif

}

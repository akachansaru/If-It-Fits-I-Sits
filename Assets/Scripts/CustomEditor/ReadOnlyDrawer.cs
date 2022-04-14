using UnityEditor;
using UnityEngine;
using CustomEditorScripts;

// From: https://answers.unity.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html?_ga=2.148018292.2119372387.1594084281-265055199.1565529348

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

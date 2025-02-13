#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FindReferenceAttribute))]
public class FindReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Check if the field is empty
        if (property.objectReferenceValue == null)
        {
            // Find the component type
            System.Type componentType = fieldInfo.FieldType;
            Component foundComponent = Object.FindAnyObjectByType(componentType) as Component;

            if (foundComponent != null)
            {
                // Assign the found component
                property.objectReferenceValue = foundComponent;
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        // Draw the default field GUI
        EditorGUI.PropertyField(position, property, label);
    }
}
#endif

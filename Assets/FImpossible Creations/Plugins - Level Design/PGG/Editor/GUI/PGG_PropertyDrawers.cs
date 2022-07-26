
using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.Generating
{
    [CustomPropertyDrawer(typeof(FieldSetup))]
    public class FieldSetup_PropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect noButtonRect = new Rect(position);
            noButtonRect.width -= 24;
            EditorGUI.PropertyField(noButtonRect, property, GUIContent.none);

            Rect buttonRect = new Rect(noButtonRect.position, new Vector2(20, 19));
            buttonRect.x += noButtonRect.width;

            if (GUI.Button(buttonRect, "→"))
            {
                if (property != null)
                    if (property.serializedObject != null)
                    {
                        FieldSetup fs = property.GetValue<FieldSetup>();
                        if (fs) FieldDesignWindow.OpenFieldSetupFileInWindow(fs);
                    }
            }

            EditorGUI.EndProperty();
        }
    }
}
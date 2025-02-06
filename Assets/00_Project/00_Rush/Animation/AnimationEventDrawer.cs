using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rush
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AnimationEvent))]
    public class AnimationEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty stateNameProperty = property.FindPropertyRelative(AnimationEventField.StateNameProperty);
            SerializedProperty stateEventProperty = property.FindPropertyRelative(AnimationEventField.StateEventProperty);

            Rect stateNameRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            Rect statEventRect = new(position.x, position.y + EditorGUIUtility.singleLineHeight + 2f, position.width, EditorGUI.GetPropertyHeight(stateEventProperty));

            EditorGUI.PropertyField(stateNameRect, stateNameProperty);
            EditorGUI.PropertyField(statEventRect, stateEventProperty, true);

            EditorGUI.EndProperty();    
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty stateEventProperty = property.FindPropertyRelative(AnimationEventField.StateEventProperty);
            return EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(stateEventProperty) + 4f;
        }
    }
#endif
}


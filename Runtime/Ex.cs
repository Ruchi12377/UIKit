using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UIKit
{
    public sealed class NonEditableAttribute : PropertyAttribute
    {
    }

    public sealed class NonEditableInPlayAttribute : PropertyAttribute
    {
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(NonEditableAttribute))]
    public sealed class NonEditableAttributeDrawer : PropertyDrawer
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

    [CustomPropertyDrawer(typeof(NonEditableInPlayAttribute))]
    public sealed class NonEditableInPlayAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (Application.isPlaying)
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = true;
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
#endif

    public static class Ex
    {
        //Build In Assetを取得する
        public static T GetResources<T>(string path) where T : Object
        {
            T _ = null;
#if UNITY_EDITOR
            _ = AssetDatabase.GetBuiltinExtraResource<T>(path);
#else
          _ = Resources.GetBuiltinResource<T>(path);
#endif
            return _;
        }
    }
}
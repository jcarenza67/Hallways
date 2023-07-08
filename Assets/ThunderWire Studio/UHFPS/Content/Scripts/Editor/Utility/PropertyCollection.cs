using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ThunderWire.Editors
{
    public class PropertyCollection : Dictionary<string, SerializedProperty>
    {
        public SerializedProperty GetRelative(string propertyPath)
        {
            string[] paths = propertyPath.Split(new char[] { '.' });
            if (TryGetValue(paths[0], out SerializedProperty pathProperty))
            {
                for (int i = 1; i < paths.Length; i++)
                {
                    pathProperty = pathProperty.FindPropertyRelative(paths[i]);
                }

                return pathProperty;
            }

            return null;
        }

        public void DrawRelative(string propertyPath)
        {
            string[] paths = propertyPath.Split(new char[] { '.' });
            if (TryGetValue(paths[0], out SerializedProperty pathProperty))
            {
                for (int i = 1; i < paths.Length; i++)
                {
                    pathProperty = pathProperty.FindPropertyRelative(paths[i]);
                }

                EditorGUILayout.PropertyField(pathProperty);
            }
        }

        public void Draw(string propertyName)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUILayout.PropertyField(property);
        }

        public bool DrawGetBool(string propertyName)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
            {
                EditorGUILayout.PropertyField(property);
                return property.boolValue;
            }

            return false;
        }

        public bool DrawToggleLeft(string propertyName)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
            {
                return property.boolValue = EditorGUILayout.ToggleLeft(property.displayName, property.boolValue);
            }

            return false;
        }

        public void DrawAll(bool indentArray = false)
        {
            foreach (var property in this)
            {
                if(indentArray && property.Value.isArray) EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(property.Value);
                if (indentArray && property.Value.isArray) EditorGUI.indentLevel--;
            }
        }

        public bool BoolValue(string propertyName)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                return property.boolValue;

            return false;
        }

        public void DrawArray(string propertyName)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
            {
                if (property.isArray) EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(property);
                if (property.isArray) EditorGUI.indentLevel--;
            }
        }

        public void Draw(string propertyName, bool includeChildren)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUILayout.PropertyField(property, includeChildren);
        }

        public void Draw(string propertyName, GUIContent label)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUILayout.PropertyField(property, label);
        }

        public bool DrawGetBool(string propertyName, GUIContent label)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
            {
                EditorGUILayout.PropertyField(property, label);
                return property.boolValue;
            }

            return false;
        }

        public void Draw(string propertyName, GUIContent label, bool includeChildren)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUILayout.PropertyField(property, label, includeChildren);
        }

        public void Draw(Rect rect, string propertyName)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUI.PropertyField(rect, property);
        }

        public void Draw(Rect rect, string propertyName, bool includeChildren)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUI.PropertyField(rect, property, includeChildren);
        }

        public void Draw(Rect rect, string propertyName, GUIContent label)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUI.PropertyField(rect, property, label);
        }

        public void Draw(Rect rect, string propertyName, GUIContent label, bool includeChildren)
        {
            if (TryGetValue(propertyName, out SerializedProperty property))
                EditorGUI.PropertyField(rect, property, label, includeChildren);
        }
    }
}
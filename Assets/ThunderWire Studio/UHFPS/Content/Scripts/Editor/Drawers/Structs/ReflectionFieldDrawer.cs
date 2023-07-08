using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomPropertyDrawer(typeof(ReflectionField))]
    public class ReflectionFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty reflectType = property.FindPropertyRelative("ReflectType");
            SerializedProperty instance = property.FindPropertyRelative("Instance");
            SerializedProperty reflectName = property.FindPropertyRelative("ReflectName");

            MonoBehaviour instanceRef = instance.objectReferenceValue as MonoBehaviour;
            ReflectionField.ReflectionType reflectionType = (ReflectionField.ReflectionType)reflectType.enumValueIndex;

            EditorDrawing.DrawHeaderWithBorder(ref position, label);

            Rect instanceRect = position;
            instanceRect.height = EditorGUIUtility.singleLineHeight;
            instanceRect.y += 2f;
            instanceRect.xMin += 2f;
            instanceRect.xMax -= instanceRect.xMax * 0.2f;
            EditorGUI.PropertyField(instanceRect, instance);

            using (new EditorGUI.DisabledGroupScope(instanceRef == null))
            {
                Rect reflectTypeRect = instanceRect;
                reflectTypeRect.xMin = reflectTypeRect.xMax + 2f;
                reflectTypeRect.xMax = position.width + EditorGUIUtility.singleLineHeight - 2f;
                EditorGUI.PropertyField(reflectTypeRect, reflectType, GUIContent.none);

                Rect reflectNameRect = position;
                reflectNameRect.height = EditorGUIUtility.singleLineHeight;
                reflectNameRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2;
                reflectNameRect.xMin += 2f;
                reflectNameRect.xMax -= 2f;

                string[] fields = GetReflectionFields(instanceRef, reflectionType);
                int selected = 0;

                if (fields.Length > 0)
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i] == reflectName.stringValue)
                        {
                            selected = i;
                            break;
                        }
                    }
                }

                selected = EditorGUI.Popup(reflectNameRect, "Reflect Name", selected, fields);
                reflectName.stringValue = fields[selected];
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 3 + 11f;
        }

        private string[] GetReflectionFields(MonoBehaviour instance, ReflectionField.ReflectionType reflectionType)
        {
            string[] fields = new string[1] { "None" };

            if (instance != null)
            {
                if (reflectionType == ReflectionField.ReflectionType.Field)
                {
                    fields = (from field in instance.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                                 where field.FieldType == typeof(bool)
                                 select field.Name).ToArray();
                }
                else if (reflectionType == ReflectionField.ReflectionType.Property)
                {
                    fields = (from prop in instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                                 where prop.PropertyType == typeof(bool)
                                 select prop.Name).ToArray();
                }
                else
                {
                    fields = (from method in instance.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                 where method.ReturnType == typeof(bool)
                                 select $"{method.ReturnType.Name} {method.Name}").ToArray();
                }
            }

            return fields;
        }
    }
}
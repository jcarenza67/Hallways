using UnityEngine;
using UnityEditor;
using ThunderWire.Attributes;

namespace ThunderWire.Editors
{
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class InspectorHeaderDrawer : Editor
    {
        bool showInspectorHeader;
        InspectorHeaderAttribute attribute;

        private void OnEnable()
        {
            var attributes = target.GetType().GetCustomAttributes(typeof(InspectorHeaderAttribute), false);

            if(attributes.Length != 0)
            {
                showInspectorHeader = true;
                attribute = (InspectorHeaderAttribute)attributes[0];
            }
        }

        public override void OnInspectorGUI()
        {
            if (showInspectorHeader)
            {
                serializedObject.Update();

                string title = attribute.title;
                string icon = attribute.icon;

                GUIContent titleGUI = new GUIContent(title);
                if (!string.IsNullOrEmpty(icon))
                    titleGUI = EditorGUIUtility.TrTextContentWithIcon(" " + title, icon);

                EditorDrawing.DrawInspectorHeader(titleGUI, target);
                if(attribute.space) EditorGUILayout.Space();

                EditorGUI.BeginChangeCheck();
                DrawPropertiesExcluding(serializedObject, "m_Script");

                if(EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();

                return;
            }

            base.OnInspectorGUI();
        }
    }
}
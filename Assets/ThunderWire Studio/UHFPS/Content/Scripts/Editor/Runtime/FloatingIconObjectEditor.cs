using UnityEngine;
using UnityEditor;
using UHFPS.Runtime;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    [CustomEditor(typeof(FloatingIconObject))]
    public class FloatingIconObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorDrawing.DrawInspectorHeader(new GUIContent("Floating Icon Object"), target);
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("An object with this script attached will be marked as a floating icon object, so a floating icon will appear following the object.", MessageType.Info);
        }
    }
}
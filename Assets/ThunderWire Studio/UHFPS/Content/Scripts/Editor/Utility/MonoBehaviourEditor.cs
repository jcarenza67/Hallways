using UnityEngine;
using UnityEditor;
using ThunderWire.Editors;

namespace UHFPS.Editors
{
    public class MonoBehaviourEditor<T> : Editor where T : MonoBehaviour
    {
        public T Target { get; private set; }
        public PropertyCollection Properties { get; private set; }

        public virtual void OnEnable()
        {
            Target = target as T;
            Properties = EditorDrawing.GetAllProperties(serializedObject);
        }
    }
}
using UnityEngine;
using ThunderWire.Attributes;

namespace UHFPS.Runtime
{
    [ExecuteAlways, InspectorHeader("Procedural Cable End")]
    public class ProceduralCableEnd : MonoBehaviour
    {
        [ReadOnly]
        public ProceduralCable proceduralCable;

        private void Update()
        {
            if (proceduralCable && !proceduralCable.manualGeneration && transform.hasChanged)
            {
                transform.hasChanged = false;
                proceduralCable.RegenerateCable();
            }
        }
    }
}
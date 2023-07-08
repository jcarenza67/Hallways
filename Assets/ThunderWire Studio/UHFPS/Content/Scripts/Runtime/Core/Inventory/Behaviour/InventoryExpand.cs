using UnityEngine;
using ThunderWire.Attributes;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    [InspectorHeader("Inventory Expand")]
    public class InventoryExpand : MonoBehaviour, IInteractStart, ISaveable
    {
        public ushort RowsToExpand;

        public void InteractStart()
        {
            
        }

        public StorableCollection OnSave()
        {
            return new StorableCollection()
            {

            };
        }

        public void OnLoad(JToken data)
        {
            
        }
    }
}
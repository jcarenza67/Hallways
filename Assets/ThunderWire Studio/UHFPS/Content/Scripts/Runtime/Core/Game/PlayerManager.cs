using UnityEngine;
using Newtonsoft.Json.Linq;
using Cinemachine;
using ThunderWire.Attributes;

namespace UHFPS.Runtime
{
    [InspectorHeader("Player Manager", space = false)]
    public class PlayerManager : MonoBehaviour, ISaveableCustom
    {
        [Header("Player References")]
        public Transform CameraHolder;
        public Camera MainCamera;
        public CinemachineVirtualCamera MainVirtualCamera;

        [Header("Script References")]
        public PlayerItemsManager PlayerItems;
        public HeadBobController HeadBobController;

        private PlayerHealth m_PlayerHealth;
        public PlayerHealth PlayerHealth
        {
            get
            {
                if (m_PlayerHealth == null)
                    m_PlayerHealth = GetComponent<PlayerHealth>();

                return m_PlayerHealth;
            }
        }

        /// <summary>
        /// This function is used to collect all local player data to be saved.
        /// </summary>
        public StorableCollection OnCustomSave()
        {
            StorableCollection data = new StorableCollection();
            data.Add("health", PlayerHealth.EntityHealth);

            StorableCollection playerItemsData = new StorableCollection();
            for (int i = 0; i < PlayerItems.PlayerItems.Count; i++)
            {
                var playerItem = PlayerItems.PlayerItems[i];
                var itemData = (playerItem as ISaveableCustom).OnCustomSave();
                playerItemsData.Add("playerItem_" + i, itemData);
            }

            data.Add("playerItems", playerItemsData);
            return data;
        }

        /// <summary>
        /// This function is used to load all stored local player data.
        /// </summary>
        public void OnCustomLoad(JToken data)
        {
            PlayerHealth.StartHealth = data["health"].ToObject<uint>();
            PlayerHealth.InitHealth();

            for (int i = 0; i < PlayerItems.PlayerItems.Count; i++)
            {
                var playerItem = PlayerItems.PlayerItems[i];
                var itemData = data["playerItems"]["playerItem_" + i];
                (playerItem as ISaveableCustom).OnCustomLoad(itemData);
            }
        }
    }
}
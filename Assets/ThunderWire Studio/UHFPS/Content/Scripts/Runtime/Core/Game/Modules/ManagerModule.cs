using UnityEngine;

namespace UHFPS.Runtime
{
    public abstract class ManagerModule : ScriptableObject
    {
        public GameManager GameManager { get; internal set; }

        protected Inventory Inventory => GameManager.Inventory;
        protected PlayerPresenceManager PlayerPresence => GameManager.PlayerPresence;

        /// <summary>
        /// Override this method to define your own behavior at Awake.
        /// </summary>
        public virtual void OnAwake() { }

        /// <summary>
        /// Override this method to define your own behavior at Start.
        /// </summary>
        public virtual void OnStart() { }

        /// <summary>
        /// Override this method to define your own behavior at Update.
        /// </summary>
        public virtual void OnUpdate() { }
    }
}
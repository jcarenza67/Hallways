using UnityEngine;
using UHFPS.Runtime;

namespace UHFPS.Scriptable
{
    public abstract class PlayerStateAsset : StateAsset
    {
        /// <summary>
        /// Initialize and get FSM Player State.
        /// </summary>
        public abstract FSMPlayerState InitState(PlayerStateMachine machine, PlayerStatesGroup group);

        /// <summary>
        /// Get a state key to help recognize which state is currently active.
        /// <br>Override it to define your own state key.</br>
        /// </summary>
        public virtual string GetStateKey() => ToString();

        /// <summary>
        /// Get FSM State display name.
        /// </summary>
        public override string ToString() => GetType().Name;
    }
}
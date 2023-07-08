using UnityEngine;

namespace UHFPS.Runtime
{
    public class FSMPlayerState : FSMState
    {
        public Transition[] Transitions { get; private set; }
        public StorableCollection StateData { get; set; }

        protected Transform cameraHolder;
        protected GameManager gameManager;
        protected PlayerStateMachine machine;
        protected CharacterController controller;
        protected PlayerItemsManager playerItems;
        protected HeadBobController headBobController;
        protected LookController cameraLook;
        protected PlayerStateMachine.ControllerState controllerState;

        /// <summary>
        /// Character controller current position.
        /// </summary>
        protected Vector3 Position
        {
            get => machine.transform.position;
            set => machine.transform.position = value;
        }

        /// <summary>
        /// Character controller center position.
        /// </summary>
        protected Vector3 CenterPosition
        {
            get => machine.ControllerCenter;
            set
            {
                Vector3 position = value;
                position -= controller.center;
                machine.transform.position = position;
            }
        }

        /// <summary>
        /// Character controller bottom position.
        /// </summary>
        protected Vector3 FeetPosition
        {
            get => machine.ControllerFeet;
            set
            {
                Vector3 position = value;
                position -= machine.ControllerFeet;
                machine.transform.position = position;
            }
        }

        /// <summary>
        /// Check if you can transition to this state when the transition is disabled.
        /// </summary>
        public virtual bool CanTransitionWhenDisabled => false;

        /// <summary>
        /// The magnitude of the movement input.
        /// </summary>
        protected float InputMagnitude => machine.Input.magnitude;

        /// <summary>
        /// Check if the character controller is on the ground.
        /// </summary>
        protected bool IsGrounded => machine.IsGrounded;

        /// <summary>
        /// Check if the stamina feature is enabled in the player.
        /// </summary>
        protected bool StaminaEnabled => machine.PlayerFeatures.EnableStamina;

        /// <summary>
        /// Check if the player has died.
        /// </summary>
        protected bool IsDead => machine.IsPlayerDead;

        public FSMPlayerState(PlayerStateMachine machine)
        {
            this.machine = machine;
            gameManager = GameManager.Instance;
            controller = machine.Controller;
            playerItems = machine.PlayerManager.PlayerItems;
            cameraHolder = machine.PlayerManager.CameraHolder;
            headBobController = machine.PlayerManager.HeadBobController;
            cameraLook = machine.LookController;
            Transitions = OnGetTransitions();
        }

        /// <summary>
        /// Get player state transitions.
        /// </summary>
        public virtual Transition[] OnGetTransitions()
        {
            return new Transition[0];
        }

        /// <summary>
        /// Change player controller height.
        /// </summary>
        public void PlayerHeightUpdate()
        {
            if (controllerState != null)
            {
                Vector3 cameraPosition = machine.SetControllerState(controllerState);
                float changeSpeed = machine.PlayerControllerSettings.StateChangeSpeed;

                Vector3 localPos = machine.PlayerManager.CameraHolder.localPosition;
                localPos = Vector3.MoveTowards(localPos, cameraPosition, Time.deltaTime * changeSpeed);
                machine.PlayerManager.CameraHolder.localPosition = localPos;
            }
        }

        /// <summary>
        /// Get player gravity force with weight.
        /// </summary>
        public float GravityForce()
        {
            float gravity = machine.PlayerControllerSettings.BaseGravity;
            float weight = machine.PlayerControllerSettings.PlayerWeight / 10f;
            return gravity - weight;
        }

        /// <summary>
        /// Apply gravity force to motion.
        /// </summary>
        public void ApplyGravity(ref Vector3 motion)
        {
            float gravityForce = GravityForce();
            motion += gravityForce * Time.deltaTime * Vector3.up;
        }

        /// <summary>
        /// Event when a player dies.
        /// </summary>
        public virtual void OnPlayerDeath() { }
    }
}
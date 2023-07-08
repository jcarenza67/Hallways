using System;
using System.Linq;
using System.Reactive.Subjects;
using UnityEngine;
using UHFPS.Input;
using UHFPS.Tools;
using UHFPS.Scriptable;

namespace UHFPS.Runtime
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerStateMachine : PlayerComponent
    {
        #region Getters / Setters
        private CharacterController m_controller;
        public CharacterController Controller
        {
            get
            {
                if (m_controller == null)
                    m_controller = GetComponent<CharacterController>();

                return m_controller;
            }
        }

        public Vector3 FeetOffset
        {
            get
            {
                float height = Controller.height;
                float skinWidth = Controller.skinWidth;
                float center = height / 2;

                return ControllerOffset switch
                {
                    PositionOffset.Ground => new Vector3(0, skinWidth, 0),
                    PositionOffset.Feet => new Vector3(0, 0, 0),
                    PositionOffset.Center => new Vector3(0, -center, 0),
                    PositionOffset.Head => new Vector3(0, -center * 2, 0),
                    _ => Controller.center
                };
            }
        }

        public Vector3 ControllerFeet
        {
            get
            {
                Vector3 position = transform.position;
                return position + FeetOffset;
            }
        }

        public Vector3 ControllerCenter
        {
            get
            {
                Vector3 position = transform.position;
                return position += Controller.center;
            }
        }

        public State? CurrentState => currentState;

        public State? PreviousState => previousState;

        public string CurrentStateKey => CurrentState?.stateData.stateAsset.GetStateKey();
        #endregion

        #region Structures
        public const string IDLE_STATE = "Idle";
        public const string WALK_STATE = "Walk";
        public const string RUN_STATE = "Run";
        public const string CROUCH_STATE = "Crouch";
        public const string JUMP_STATE = "Jump";
        public const string DEATH_STATE = "Death";

        public const string LADDER_STATE = "Ladder";
        public const string ZIPLINE_STATE = "Zipline";
        public const string SLIDING_STATE = "Sliding";
        public const string PUSHING_STATE = "Pushing";

        [Serializable]
        public sealed class BasicSettings
        {
            public float WalkSpeed = 3;
            public float RunSpeed = 7;
            public float CrouchSpeed = 2;
            public float JumpHeight = 1;
        }

        [Serializable]
        public sealed class ControllerFeatures
        {
            public bool EnableJump = true;
            public bool EnableRun = true;
            public bool EnableCrouch = true;
            public bool EnableStamina = false;
            public bool RunToggle = false;
            public bool CrouchToggle = false;
            public bool NormalizeMovement = false;
        }

        [Serializable]
        public sealed class StaminaSettings
        {
            public float JumpExhaustion = 1f;
            public float RunExhaustionSpeed = 1f;
            public float StaminaRegenSpeed = 1f;
            public float RegenerateAfter = 2f;
        }

        [Serializable]
        public sealed class ControllerSettings
        {
            public float BaseGravity = -9.81f;
            public float PlayerWeight = 70f;
            public float AntiBumpFactor = 4.5f;
            public float WallRicochet = 0.1f;
            public float StateChangeSpeed = 3f;
        }

        [Serializable]
        public sealed class ControllerState
        {
            public float ControllerHeight;
            public Vector3 CameraOffset;
        }

        public struct State
        {
            public PlayerStateData stateData;
            public FSMPlayerState fsmState;
        }
        #endregion

        public enum PositionOffset { Ground, Feet, Center, Head }

        public PlayerStatesGroup StatesAsset;
        public PlayerStatesGroup StatesAssetRuntime;

        public LayerMask SurfaceMask;
        public PositionOffset ControllerOffset;

        public BasicSettings PlayerBasicSettings;
        public ControllerFeatures PlayerFeatures;
        public StaminaSettings PlayerStamina;
        public ControllerSettings PlayerControllerSettings;

        public ControllerState StandingState;
        public ControllerState CrouchingState;

        public Vector2 Input;
        public Vector3 Motion;

        public ControllerColliderHit ControllerHit { get; private set; }
        public BehaviorSubject<float> Stamina { get; set; } = new(1f);
        public bool IsGrounded { get; private set; }
        public bool IsPlayerDead;

        private MultiKeyDictionary<string, Type, State> playerStates;
        private State? currentState;
        private State? previousState;
        private bool stateEntered;
        private float staminaRegenTime;

        private void Awake()
        {
            playerStates = new MultiKeyDictionary<string, Type, State>();
            StatesAssetRuntime = Instantiate(StatesAsset);

            if (StatesAsset != null)
            {
                // initialize all states
                foreach (var playerState in StatesAssetRuntime.GetStates(this))
                {
                    Type stateType = playerState.stateData.stateAsset.GetType();
                    string stateKey = playerState.stateData.stateAsset.GetStateKey();
                    playerStates.Add(stateKey, stateType, playerState);
                }

                // select initial player state
                if (playerStates.Count > 0)
                {
                    stateEntered = false;
                    ChangeState(playerStates.subDictionary.Keys.First());
                }
            }
        }

        private void Update()
        {
            if (isEnabled) GetInput();
            else Input = Vector2.zero;

            // player death event
            if (currentState != null && !IsPlayerDead && PlayerManager.PlayerHealth.IsDead)
            {
                currentState?.fsmState.OnPlayerDeath();
                IsPlayerDead = true;
            }

            if (!stateEntered)
            {
                // enter state
                currentState?.fsmState.OnStateEnter();
                stateEntered = true;
            }
            else if (currentState != null)
            {
                // update state
                currentState?.fsmState.OnStateUpdate();

                // check state transitions
                if (currentState.Value.fsmState.Transitions != null)
                {
                    foreach (var transition in currentState.Value.fsmState.Transitions)
                    {
                        if (transition.Value && currentState.GetType() != transition.NextState)
                        {
                            if (transition.NextState == null) ChangeToPreviousState();
                            else ChangeState(transition.NextState);
                            break;
                        }
                    }
                }
            }

            // regenerate player stamina
            if (PlayerFeatures.EnableStamina)
            {
                bool runHold = InputManager.ReadButton(Controls.SPRINT);
                if (IsCurrent(RUN_STATE) || IsCurrent(JUMP_STATE) || runHold)
                {
                    staminaRegenTime = PlayerStamina.RegenerateAfter;
                }
                else if(staminaRegenTime > 0f)
                {
                    staminaRegenTime -= Time.deltaTime;
                }
                else if(Stamina.Value < 1f)
                {
                    float stamina = Stamina.Value;
                    stamina = Mathf.MoveTowards(stamina, 1f, Time.deltaTime * PlayerStamina.StaminaRegenSpeed);
                    Stamina.OnNext(stamina);
                    staminaRegenTime = 0f;
                }
            }

            // apply movement direction
            if(Controller.enabled) IsGrounded = (Controller.Move(Motion * Time.deltaTime) & CollisionFlags.Below) != 0;
        }

        private void FixedUpdate()
        {
            if (currentState != null)
            {
                // update state
                currentState?.fsmState.OnStateFixedUpdate();
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!IsGrounded)
            {
                Vector3 normal = hit.normal;
                if (normal.y > 0) return;

                Vector3 ricochet = Vector3.Reflect(Motion, normal);
                ricochet.y = Motion.y;

                float ricochetDot = Mathf.Clamp01(Vector3.Dot(ricochet.normalized, Motion.normalized));
                float wallDot = Mathf.Clamp01(Vector3.Dot(Motion.normalized, -normal));

                float ricochetMul = Mathf.Lerp(1f, PlayerControllerSettings.WallRicochet, wallDot);
                ricochet *= ricochetMul;

                Vector3 newMotion = Vector3.Lerp(ricochet, Motion, ricochetDot);
                newMotion.y = Motion.y;

                Motion = newMotion;
            }

            ControllerHit = hit;
        }

        /// <summary>
        /// Calculate movement input vector.
        /// </summary>
        private void GetInput()
        {
            Input = Vector2.zero;
            if (InputManager.ReadInput(Controls.MOVEMENT, out Vector2 _rawInput))
            {
                if (PlayerFeatures.NormalizeMovement)
                {
                    _rawInput.y = _rawInput.y > 0.1f ? 1 : _rawInput.y < -0.1f ? -1 : 0;
                    _rawInput.x = _rawInput.x > 0.1f ? 1 : _rawInput.x < -0.1f ? -1 : 0;
                }
                Input = _rawInput;
            }
        }

        /// <summary>
        /// Set player controller state.
        /// </summary>
        public Vector3 SetControllerState(ControllerState state)
        {
            float height = state.ControllerHeight;
            float skinWidth = Controller.skinWidth;
            float center = height / 2;

            Vector3 controllerCenter = ControllerOffset switch
            {
                PositionOffset.Ground => new Vector3(0, center + skinWidth, 0),
                PositionOffset.Feet => new Vector3(0, center, 0),
                PositionOffset.Center => new Vector3(0, 0, 0),
                PositionOffset.Head => new Vector3(0, -center, 0),
                _ => Controller.center
            };

            Controller.height = height;
            Controller.center = controllerCenter;

            Vector3 cameraTop = state.CameraOffset;
            cameraTop.y += center + controllerCenter.y;

            return cameraTop;
        }

        /// <summary>
        /// Change player FSM state.
        /// </summary>
        public void ChangeState<TState>() where TState : PlayerStateAsset
        {
            if (playerStates.TryGetValue(typeof(TState), out State state))
            {
                if (!isEnabled && !state.fsmState.CanTransitionWhenDisabled)
                    return;

                if ((currentState == null || !currentState.Value.Equals(state)) && state.stateData.isEnabled)
                {
                    currentState?.fsmState.OnStateExit();
                    if (currentState.HasValue) previousState = currentState;
                    currentState = state;
                    stateEntered = false;
                }
                return;
            }

            throw new MissingReferenceException($"Could not find a state with type '{typeof(TState).Name}'");
        }

        /// <summary>
        /// Change player FSM state.
        /// </summary>
        public void ChangeState(Type nextState)
        {
            if (playerStates.TryGetValue(nextState, out State state))
            {
                if (!isEnabled && !state.fsmState.CanTransitionWhenDisabled)
                    return;

                if ((currentState == null || !currentState.Value.Equals(state)) && state.stateData.isEnabled)
                {
                    currentState?.fsmState.OnStateExit();
                    if (currentState.HasValue) previousState = currentState;
                    currentState = state;
                    stateEntered = false;
                }
                return;
            }

            throw new MissingReferenceException($"Could not find a state with type '{nextState.Name}'");
        }

        /// <summary>
        /// Change player FSM state.
        /// </summary>
        public void ChangeState(string nextState)
        {
            if (playerStates.TryGetValue(nextState, out State state))
            {
                if (!isEnabled && !state.fsmState.CanTransitionWhenDisabled)
                    return;

                if ((currentState == null || !currentState.Value.Equals(state)) && state.stateData.isEnabled)
                {
                    currentState?.fsmState.OnStateExit();
                    if (currentState.HasValue) previousState = currentState;
                    currentState = state;
                    stateEntered = false;
                }
                return;
            }

            throw new MissingReferenceException($"Could not find a state with key '{nextState}'");
        }

        /// <summary>
        /// Change player FSM state and set the state data.
        /// </summary>
        public void ChangeState(string nextState, StorableCollection stateData)
        {
            if (playerStates.TryGetValue(nextState, out State state))
            {
                if (!isEnabled && !state.fsmState.CanTransitionWhenDisabled)
                    return;

                if ((currentState == null || !currentState.Value.Equals(state)) && state.stateData.isEnabled)
                {
                    currentState?.fsmState.OnStateExit();
                    if (currentState.HasValue) previousState = currentState;
                    state.fsmState.StateData = stateData;
                    currentState = state;
                    stateEntered = false;
                }
                return;
            }

            throw new MissingReferenceException($"Could not find a state with key '{nextState}'");
        }

        /// <summary>
        /// Change player FSM state to previous state.
        /// </summary>
        public void ChangeToPreviousState()
        {
            if (previousState != null && !currentState.Value.Equals(previousState) && previousState.Value.stateData.isEnabled)
            {
                if (!isEnabled && !previousState.Value.fsmState.CanTransitionWhenDisabled)
                    return;

                currentState?.fsmState.OnStateExit();
                State temp = currentState.Value;
                currentState = previousState;
                previousState = temp;
                stateEntered = false;
            }
        }

        /// <summary>
        /// Check if current state is of the specified type.
        /// </summary>
        public bool IsCurrent(Type stateType)
        {
            return currentState.Value.stateData.stateAsset.GetType() == stateType;
        }

        /// <summary>
        /// Check if current state matches the specified state key.
        /// </summary>
        public bool IsCurrent(string stateKey)
        {
            return currentState.Value.stateData.stateAsset.GetStateKey() == stateKey;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(ControllerFeet, 0.02f);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(ControllerCenter, 0.05f);

            if (currentState != null)
                currentState?.fsmState.OnDrawGizmos();
        }
    }
}
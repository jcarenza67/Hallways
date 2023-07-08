using System;
using UnityEngine;
using UnityEngine.Playables;
using UHFPS.Input;
using UHFPS.Scriptable;

namespace UHFPS.Runtime.States
{
    public class CutsceneStateAsset : PlayerStateAsset
    {
        public float ToCutsceneTime = 0.2f;

        public override FSMPlayerState InitState(PlayerStateMachine machine, PlayerStatesGroup group)
        {
            return new CutscenePlayerState(machine, this);
        }

        public override string GetStateKey() => "Cutscene";

        public override string ToString() => "Cutscene";

        public class CutscenePlayerState : FSMPlayerState
        {
            private readonly CutsceneStateAsset State;
            private CutsceneModule cutsceneModule;

            private Vector3 targetPosition;
            private Vector2 targetLook;
            private PlayableDirector cutscene;
            private Action completedEvent;

            private Vector3 startingPosition;
            private bool cutsceneStart;
            private bool cutsceneEnd;

            private float t = 0f;
            private float tVel;

            public CutscenePlayerState(PlayerStateMachine machine, CutsceneStateAsset stateAsset) : base(machine) 
            {
                State = stateAsset;
            }

            public override void OnStateEnter()
            {
                cutsceneModule = GameManager.Module<CutsceneModule>();
                targetPosition = (Vector3)StateData["position"];
                targetLook = (Vector2)StateData["look"];
                cutscene = (PlayableDirector)StateData["cutscene"];
                completedEvent = (Action)StateData["event"];

                cutsceneStart = false;
                cutsceneEnd = false;

                InputManager.ResetToggledButtons();
                startingPosition = Position;
                machine.Motion = Vector3.zero;
                t = 0f;

                playerItems.IsItemsUsable = false;
            }

            public override void OnStateExit()
            {
                playerItems.IsItemsUsable = true;
                cameraLook.ResetCustomLerp();
                machine.Motion = Vector3.zero;
            }

            public override void OnStateUpdate()
            {
                if (cutsceneStart) return;
                t = Mathf.SmoothDamp(t, 1.001f, ref tVel, State.ToCutsceneTime);

                if (t < 1f)
                {
                    Position = Vector3.Lerp(startingPosition, targetPosition, t);
                    cameraLook.CustomLerp(targetLook, t);
                }
                else if(!cutsceneStart)
                {
                    cutsceneModule.PlayCutscene(cutscene, () => 
                    {
                        cutsceneEnd = true;
                        completedEvent.Invoke();
                    });
                    cutsceneStart = true;
                }

                controllerState = machine.StandingState;
                PlayerHeightUpdate();
            }

            public override Transition[] OnGetTransitions()
            {
                return new Transition[]
                {
                    Transition.To<WalkingStateAsset>(() => cutsceneEnd)
                };
            }
        }
    }
}
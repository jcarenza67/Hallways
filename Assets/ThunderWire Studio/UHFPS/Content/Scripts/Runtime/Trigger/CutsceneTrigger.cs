using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Cinemachine;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    public class CutsceneTrigger : MonoBehaviour, ISaveable
    {
        public enum CutsceneTypeEnum { CameraCutscene, PlayerCutscene }

        public CutsceneTypeEnum CutsceneType;
        public PlayableDirector Cutscene;

        public CinemachineVirtualCamera CutsceneCamera;
        public float CutsceneFadeSpeed;

        public Vector3 InitialPosition;
        public Vector2 InitialLook;

        public UnityEvent OnCutsceneStart;
        public UnityEvent OnCutsceneEnd;

        private CutsceneModule cutscene;
        private PlayerStateMachine player;
        private bool isPlayed;

        private void Awake()
        {
            cutscene = GameManager.Module<CutsceneModule>();
            PlayerPresenceManager playerPresence = PlayerPresenceManager.Instance;
            player = playerPresence.Player.GetComponent<PlayerStateMachine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isPlayed)
            {
                if (CutsceneType == CutsceneTypeEnum.CameraCutscene)
                {
                    cutscene.PlayCutscene(Cutscene, CutsceneCamera.gameObject, CutsceneFadeSpeed, () => OnCutsceneEnd?.Invoke());
                }
                else
                {
                    player.ChangeState("Cutscene", 
                        new StorableCollection() {
                            { "position", InitialPosition },
                            { "look", InitialLook },
                            { "cutscene", Cutscene },
                            { "event", new Action(() => OnCutsceneEnd?.Invoke()) }
                        });
                }

                OnCutsceneStart?.Invoke();
                isPlayed = true;
            }
        }

        public StorableCollection OnSave()
        {
            return new StorableCollection()
            {
                { nameof(isPlayed), isPlayed }
            };
        }

        public void OnLoad(JToken data)
        {
            isPlayed = (bool)data[nameof(isPlayed)];
        }
    }
}
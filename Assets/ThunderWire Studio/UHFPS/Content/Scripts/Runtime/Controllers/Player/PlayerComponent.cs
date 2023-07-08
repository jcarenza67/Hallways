using System;
using UnityEngine;
using Cinemachine;

namespace UHFPS.Runtime
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        protected bool isEnabled = true;

        public virtual void SetEnabled(bool enabled)
        {
            isEnabled = enabled;
        }

        [NonSerialized]
        private CharacterController playerCollider;
        public CharacterController PlayerCollider
        {
            get
            {
                if (playerCollider == null)
                    playerCollider = transform.root.GetComponent<CharacterController>();

                return playerCollider;
            }
        }

        [NonSerialized]
        private PlayerStateMachine playerStateMachine;
        public PlayerStateMachine PlayerStateMachine
        {
            get
            {
                if (playerStateMachine == null)
                    playerStateMachine = transform.root.GetComponent<PlayerStateMachine>();

                return playerStateMachine;
            }
        }

        [NonSerialized]
        private LookController lookController;
        public LookController LookController
        {
            get
            {
                if (lookController == null)
                    lookController = transform.root.GetComponentInChildren<LookController>();

                return lookController;
            }
        }

        [NonSerialized]
        private ExamineController examineController;
        public ExamineController ExamineController
        {
            get
            {
                if (examineController == null)
                    examineController = transform.root.GetComponentInChildren<ExamineController>();

                return examineController;
            }
        }

        [NonSerialized]
        private PlayerManager playerManager;
        public PlayerManager PlayerManager
        {
            get
            {
                if (playerManager == null)
                    playerManager = transform.root.GetComponent<PlayerManager>();

                return playerManager;
            }
        }

        public Camera MainCamera => PlayerManager.MainCamera;
        public CinemachineVirtualCamera VirtualCamera => PlayerManager.MainVirtualCamera;
    }
}
using UnityEngine;

namespace UHFPS.Runtime
{
    public class SwayEffect : MonoBehaviour
    {
        [Header("Look Sway")]
        public float LookSway = 10f;
        public float AimingLookSway = 10f;
        public float WalkSway = 10f;

        [Header("Sway Settings")]
        public float MaxSway = 10f;
        public float SwaySpeed = 5f;

        public bool isAiming = false;

        private LookController lookController;
        private PlayerStateMachine playerStateMachine;

        private void Awake()
        {
            lookController = transform.GetComponentInParent<LookController>();
            playerStateMachine = transform.GetComponentInParent<PlayerStateMachine>();
        }

        private void Update()
        {
            float magnitude = playerStateMachine.PlayerCollider.velocity.magnitude;
            float inputX = playerStateMachine.Input.x * -1;
            float inputY = playerStateMachine.Input.y;

            float vertical = lookController.DeltaInput.y * -1;
            float horizontal = lookController.DeltaInput.x;

            vertical *= isAiming ? AimingLookSway : LookSway;
            horizontal *= isAiming ? AimingLookSway : LookSway;

            vertical = Mathf.Clamp(vertical, -MaxSway, MaxSway);
            horizontal = Mathf.Clamp(horizontal, -MaxSway, MaxSway);

            magnitude = Mathf.Clamp(magnitude, -1, 1);
            float sideway = inputX * magnitude * WalkSway;
            float forward = inputY * magnitude * WalkSway;

            Vector3 final = new Vector3(vertical + forward, horizontal, sideway);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(final), Time.deltaTime * SwaySpeed);
        }
    }
}
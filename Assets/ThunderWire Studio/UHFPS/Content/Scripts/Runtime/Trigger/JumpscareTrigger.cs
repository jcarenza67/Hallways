using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UHFPS.Tools;
using Newtonsoft.Json.Linq;

namespace UHFPS.Runtime
{
    public class JumpscareTrigger : MonoBehaviour, ISaveable
    {
        public Animator Animator;
        public bool PlayOnEnabled = false;
        public string JumpscareState = "Jumpscare";

        public float WobbleAmplitudeGain = 1f;
        public float WobbleFrequencyGain = 1f;
        public float WobbleDuration = 0.2f;
        public float SanityDuration = 2f;

        public SoundClip JumpscareSound;

        public UnityEvent OnJumpscareStarted;
        public UnityEvent OnJumpscareEnded;

        private bool jumpscareStarted = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !jumpscareStarted)
            {
                StartJumpscare();
            }
        }

        public void StartJumpscare()
        {
            if (jumpscareStarted) 
                return;

            OnJumpscareStarted?.Invoke();

            if (PlayOnEnabled) Animator.gameObject.SetActive(true);
            else Animator.SetTrigger(JumpscareState);

            GameManager.Module<JumpscareModule>().ApplyJumpscareEffect(this);
            GameTools.PlayOneShot2D(transform.position, JumpscareSound, "Jumpscare Sound");

            StartCoroutine(OnJumpscare());
            jumpscareStarted = true;
        }

        IEnumerator OnJumpscare()
        {
            yield return new WaitForAnimatorStateExit(Animator, JumpscareState);
            OnJumpscareEnded?.Invoke();
        }

        public StorableCollection OnSave()
        {
            return new StorableCollection()
            {
                { nameof(jumpscareStarted), jumpscareStarted }
            };
        }

        public void OnLoad(JToken data)
        {
            jumpscareStarted = (bool)data[nameof(jumpscareStarted)];
        }
    }
}
using UnityEngine;

public class FootstepSoundScript : MonoBehaviour
{
    public AudioClip walkingSound;
    public AudioClip runningSound;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 5.0f;
    public float footstepCooldown = 0.5f; // Adjust this value to control the footstep sound delay

    private CharacterController characterController;
    private AudioSource walkingAudioSource;
    private AudioSource runningAudioSource;
    private bool canPlayFootstep = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        walkingAudioSource = gameObject.AddComponent<AudioSource>();
        runningAudioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the player is moving and play the corresponding footstep sound
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            if (characterController.velocity.magnitude <= walkingSpeed)
            {
                if (canPlayFootstep)
                {
                    PlayFootstepSound(walkingAudioSource, walkingSound);
                }
            }
            else if (characterController.velocity.magnitude > walkingSpeed)
            {
                if (canPlayFootstep)
                {
                    PlayFootstepSound(runningAudioSource, runningSound);
                }
            }
        }
    }

    private void PlayFootstepSound(AudioSource audioSource, AudioClip soundClip)
    {
        audioSource.PlayOneShot(soundClip);
        canPlayFootstep = false;
        Invoke(nameof(ResetFootstepCooldown), footstepCooldown);
    }

    private void ResetFootstepCooldown()
    {
        canPlayFootstep = true;
    }
}

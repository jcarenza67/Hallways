using UnityEngine;

public class FootstepSoundScript : MonoBehaviour
{
    public AudioClip walkingSound;
    public AudioClip runningSound;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 5.0f;

    public float walkingFootstepCooldown = 0.5f;
    public float runningFootstepCooldown = 0.3f;

    public float walkingVolume = 1.0f;
    public float runningVolume = 1.0f;

    private CharacterController characterController;
    private AudioSource walkingAudioSource;
    private AudioSource runningAudioSource;
    private bool canPlayFootstep = true;
    private float currentFootstepCooldown;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        walkingAudioSource = gameObject.AddComponent<AudioSource>();
        walkingAudioSource.volume = walkingVolume;

        runningAudioSource = gameObject.AddComponent<AudioSource>();
        runningAudioSource.volume = runningVolume;
    }

    void Update()
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            if (characterController.velocity.magnitude <= walkingSpeed)
            {
                if (canPlayFootstep)
                {
                    currentFootstepCooldown = walkingFootstepCooldown;
                    PlayFootstepSound(walkingAudioSource, walkingSound);
                }
            }
            else if (characterController.velocity.magnitude > walkingSpeed)
            {
                if (canPlayFootstep)
                {
                    currentFootstepCooldown = runningFootstepCooldown;
                    PlayFootstepSound(runningAudioSource, runningSound);
                }
            }
        }
    }

    private void PlayFootstepSound(AudioSource audioSource, AudioClip soundClip)
    {
        audioSource.PlayOneShot(soundClip);
        canPlayFootstep = false;
        Invoke(nameof(ResetFootstepCooldown), currentFootstepCooldown);
    }

    private void ResetFootstepCooldown()
    {
        canPlayFootstep = true;
    }
}

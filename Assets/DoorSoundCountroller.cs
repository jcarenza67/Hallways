using UnityEngine;

public class DoorSoundController : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource you want to control
    public SimpleOpenClose door; // Reference to your SimpleOpenClose script

    void Update()
    {
        // Check if the door has been opened
        if (door.objectOpen)
        {
            // If the door is open, stop the sound
            audioSource.Stop();
        }
    }
}


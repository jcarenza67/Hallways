using UnityEngine;

public class StopAudioOnOpen : MonoBehaviour
{
    public AudioSource audioSource;
    public SimpleOpenClose doorController;

    void Update()
    {
        if(doorController.objectOpen)
        {
            audioSource.Stop();
        }
    }
}

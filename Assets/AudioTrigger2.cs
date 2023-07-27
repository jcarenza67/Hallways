using UnityEngine;

public class AudioTrigger2 : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource audioSource2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            audioSource2.Play();
        }
    }
}
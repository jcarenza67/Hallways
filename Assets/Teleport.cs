using UnityEngine;

public class Teleport : MonoBehaviour
{
    public AudioSource bathroomAudio;
    public BathroomDoorScript bathroomDoor;
    public Transform teleportTarget;
    public void TeleportPlayer()
    {
        if (teleportTarget != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = teleportTarget.position;
                player.transform.position = teleportTarget.position;
            }
            else
            {
                Debug.LogError("No GameObject with the Player tag found in the scene.");
            }
        }
        else
        {
            Debug.LogError("Teleport target is not assigned.");
        }
    }
}

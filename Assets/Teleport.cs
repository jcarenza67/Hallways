using UnityEngine;

public class Teleport : MonoBehaviour
{
    public AudioSource bathroomAudio;
    public BathroomDoorScript bathroomDoor;
    public Transform teleportTarget;
    public Transform teleportOrigin;
    public void TeleportPlayer()
    {
        if (teleportTarget != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Vector3 offset = player.transform.position - teleportOrigin.position;
                player.transform.position = teleportTarget.position + offset;
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

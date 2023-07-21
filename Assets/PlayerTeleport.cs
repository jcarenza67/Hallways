using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinalDoor"))
        {
            transform.position = teleportTarget.position;
        }
    }
}


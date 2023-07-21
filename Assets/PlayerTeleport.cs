using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public CharacterController controller; // Assign your player's Character Controller in the inspector

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");

        if (other.gameObject.CompareTag("FinalDoor"))
        {
            Debug.Log("Teleporting player");
            controller.enabled = false;
            transform.position = teleportTarget.position;
            transform.rotation = teleportTarget.rotation;
            controller.enabled = true;
        }
    }
}

using UnityEngine;

public class DoorTriggerScript : MonoBehaviour
{
    private Teleport teleportComponent;

    private void Awake()
    {
        teleportComponent = FindObjectOfType<Teleport>(); // Assumes there's only one Teleport component in the scene.
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            
            teleportComponent.TeleportPlayer();
        }
    }
}

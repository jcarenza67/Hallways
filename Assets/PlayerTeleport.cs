using UnityEngine;
using System.Collections.Generic;

public class PlayerTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public CharacterController controller; // Assign your player's Character Controller in the inspector
    public List<SimpleOpenClose> allDoors = new List<SimpleOpenClose>(); // Add this line here

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");

        if (other.gameObject.CompareTag("FinalDoor"))
        {
            Debug.Log("Teleporting player");
            controller.enabled = false;
            transform.position = teleportTarget.position;
            transform.rotation = teleportTarget.rotation;

            // Close all doors that are open
            foreach (SimpleOpenClose door in allDoors)
            {
                if (door.objectOpen)
                {
                    door.ObjectClicked();
                }
            }
            
            controller.enabled = true;
        }
    }
}


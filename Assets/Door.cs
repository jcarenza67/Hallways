using UnityEngine;
using UHFPS.Runtime;

public class Door : MonoBehaviour, IOpenable
{
    [Header("Door Settings")]
    public bool isInitiallyOpen = false;
    public float openSpeed = 1f;

    private bool isOpen;
    private float currentOpenAmount;

    private void Start()
    {
        isOpen = isInitiallyOpen;
        currentOpenAmount = isOpen ? 1f : 0f;
        UpdateDoorState();
    }

    public void StartOpening()
    {
        // When opening starts, set 'isOpen' to true
        isOpen = true;
    }

    public void UpdateOpening(float intensity)
    {
        // When opening updates, adjust 'currentOpenAmount' based on 'intensity' and 'openSpeed'
        if (isOpen)
        {
            currentOpenAmount += intensity * openSpeed * Time.deltaTime;
            currentOpenAmount = Mathf.Clamp01(currentOpenAmount); // Ensures value is between 0 and 1
        }
        UpdateDoorState();
    }

    public void StopOpening()
    {
        // When opening stops, set 'isOpen' to false
        isOpen = false;
    }

    private void UpdateDoorState()
    {
        // Update the actual visual/physical state of the door based on 'currentOpenAmount'
        // This will depend on how your specific door is implemented
        // It might involve rotating a hinge, moving the door position, or adjusting an animation state
    }
}
// this was for the old build
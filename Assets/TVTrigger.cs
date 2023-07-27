using UnityEngine;

public class TVTrigger : MonoBehaviour
{
    public FlickeringLight2 tvFlickeringLight;
    public TVEmissionController tvEmissionController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tvFlickeringLight.TurnOnLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tvFlickeringLight.TurnOffLight();
        }
    }

}

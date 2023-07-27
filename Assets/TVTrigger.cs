using UnityEngine;

public class TVTrigger : MonoBehaviour
{
    public FlickeringLight tvFlickeringLight;
    public TVEmissionController tvEmissionController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tvFlickeringLight.lightSource.enabled = true;
            tvEmissionController.enabled = true;
        }
    }
}

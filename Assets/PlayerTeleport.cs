using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public Image fadeImage;
    public CharacterController controller;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.gameObject.CompareTag("FinalDoor"))
        {
            Debug.Log("Teleporting");
            StartCoroutine(TeleportAndFade());
        }
    }

    IEnumerator TeleportAndFade()
    {
        for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime)
        {
            Color c = fadeImage.color;
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }

        controller.enabled = false;
        transform.position = teleportTarget.position;
        transform.rotation = teleportTarget.rotation;
        controller.enabled = true;

        for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime)
        {
            Color c = fadeImage.color;
            c.a = t;
            fadeImage.color = c;
            yield return null;
        }
    }
}



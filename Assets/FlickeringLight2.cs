using System.Collections;
using UnityEngine;

public class FlickeringLight2 : MonoBehaviour
{
    public Light lightSource2;
    public bool lightOn = false;
    public float minWaitTime = 0.1f;
    public float maxWaitTime = 0.5f;

    private Coroutine flickerCoroutine;

    private void Start()
    {
        lightSource2.enabled = lightOn;
        if (lightOn)
        {
            flickerCoroutine = StartCoroutine(Flicker());
        }
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            lightSource2.enabled = !lightSource2.enabled;
        }
    }

    public void TurnOnLight()
    {
        lightOn = true;
        lightSource2.enabled = true;
        if (flickerCoroutine == null)
        {
            flickerCoroutine = StartCoroutine(Flicker());
        }
    }

    public void TurnOffLight()
    {
        lightOn = false;
        lightSource2.enabled = false;
        if (flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
  public Light lightSource;
    public float minWaitTime = 0.1f;
    public float maxWaitTime = 0.5f;

    void Start()
    {
        StartCoroutine(StartFlicker());
    }

    IEnumerator StartFlicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            lightSource.enabled = !lightSource.enabled;
        }
    }
}

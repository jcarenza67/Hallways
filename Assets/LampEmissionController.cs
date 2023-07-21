using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampEmissionController : MonoBehaviour
{
    public Material lampMaterial; // assign in Inspector
    private Material initialMaterial;
    private Material flickerMaterial;
    public Color initialEmissionColor = Color.black; // initial emission when light is off
    public float maxEmission = 1.0f; // max emission when light is on
    public float minEmission = 0.0f; // min emission when light is off
    public FlickeringLight flickeringLight; // assign in Inspector

    private Color baseColor;
    private bool isFlickering = false;

    private void Start()
    {
        initialMaterial = GetComponent<Renderer>().material;
        // get the base color of the material's emission
        baseColor = lampMaterial.GetColor("_EmissionColor");
        lampMaterial.SetColor("_EmissionColor", initialEmissionColor);
    }

    private void Update()
    {
        if (flickeringLight.lightSource.enabled)
        {
            if (!isFlickering)
            {
                isFlickering = true;
                flickerMaterial = new Material(lampMaterial);
                GetComponent<Renderer>().material = flickerMaterial;
            }
            float emission = maxEmission;
            flickerMaterial.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
        }
        else
        {
            if (isFlickering)
            {
                isFlickering = false;
                GetComponent<Renderer>().material = initialMaterial;
            }
            float emission = minEmission;
            initialMaterial.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
        }
    }
}


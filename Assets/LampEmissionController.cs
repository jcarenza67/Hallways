using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampEmissionController : MonoBehaviour
{
    public Material lampMaterial; // assign in Inspector
    public Color initialEmissionColor = Color.black; // initial emission when light is off
    public float maxEmission = 1.0f; // max emission when light is on
    public float minEmission = 0.0f; // min emission when light is off
    public FlickeringLight flickeringLight; // assign in Inspector

    private Color baseColor;

    private void Start()
    {
        // get the base color of the material's emission
        baseColor = lampMaterial.GetColor("_EmissionColor");
        lampMaterial.SetColor("_EmissionColor", initialEmissionColor);
    }

    private void Update()
    {
        float emission = flickeringLight.lightSource.enabled ? maxEmission : minEmission;
        lampMaterial.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
    }
}

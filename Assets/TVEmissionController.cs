using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVEmissionController : MonoBehaviour
{
    public Material tvMaterial;
    public FlickeringLight flickeringLight;
    private Color baseColor;
    public Color initialEmissionColor = Color.black;
    public float maxEmission = 1.0f;
    public float minEmission = 0.0f;

    private void Start()
    {
        baseColor = tvMaterial.GetColor("_EmissionColor");
        tvMaterial.SetColor("_EmissionColor", initialEmissionColor);
    }

    private void Update()
    {
        if (flickeringLight.lightSource.enabled)
        {
            float emission = maxEmission;
            tvMaterial.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
        }
        else
        {
            float emission = minEmission;
            tvMaterial.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
        }
    }
}

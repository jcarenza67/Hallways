using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVEmissionController : MonoBehaviour
{
    public Material tvMaterial;
    public FlickeringLight2 flickeringLight2;
    public AudioSource audioSource;
    private MaterialPropertyBlock propertyBlock;
    public Renderer rend;
    private Color baseColor;
    public Color initialEmissionColor = Color.black;
    public float maxEmission = 1.0f;
    public float minEmission = 0.0f;

    private void Start()
    {
        baseColor = tvMaterial.GetColor("_EmissionColor");
        tvMaterial.SetColor("_EmissionColor", initialEmissionColor);
        propertyBlock = new MaterialPropertyBlock();

        float emission = minEmission;
        propertyBlock.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
        rend.SetPropertyBlock(propertyBlock);

        // Initially, the TV is off
        flickeringLight2.TurnOffLight();
    }

    private void Update()
    {
        float emission;
        if (flickeringLight2.lightSource2.enabled)
        {
            emission = maxEmission;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            emission = minEmission;
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        propertyBlock.SetColor("_EmissionColor", baseColor * Mathf.LinearToGammaSpace(emission));
        rend.SetPropertyBlock(propertyBlock);
    }

    public void TurnOnTV()
    {
        flickeringLight2.TurnOnLight();
    }

    public void TurnOffTV()
    {
        flickeringLight2.TurnOffLight();
    }
}

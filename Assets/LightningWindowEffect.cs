using System.Collections;
using System.Linq;
using UnityEngine;

public class LightningWindowEffect : MonoBehaviour
{
    public Material windowMaterial;
    public Color emissionColor = Color.white; // Set this to your desired color in the inspector
    public float minDelay = 2.0f;
    public float maxDelay = 5f;
    public float lightningDuration = 0.1f; // Adjust this for your desired flash duration
    public AudioSource thunderAudioSource; // Assign the audio source for the thunder sound in the inspector

    private Color originalEmissionColor;
    private float[] audioSpectrum = new float[256];

    private void Start()
    {
        // Save the original emission color
        originalEmissionColor = windowMaterial.GetColor("_EmissionColor");

        // Start the lightning coroutine
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        while (true)
        {
            // Get the audio spectrum
            thunderAudioSource.GetSpectrumData(audioSpectrum, 0, FFTWindow.BlackmanHarris);

            // If the maximum value in the audio spectrum is above a certain threshold, don't flash
            if (audioSpectrum.Max() > 0.1f)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

                // First flash
                windowMaterial.SetColor("_EmissionColor", emissionColor);
                yield return new WaitForSeconds(lightningDuration);

                // Return to original emission color
                windowMaterial.SetColor("_EmissionColor", originalEmissionColor);
                yield return new WaitForSeconds(lightningDuration / 2); // You can adjust this delay to fit your needs

                // Second flash (you can add as many additional flashes as you want following this pattern)
                windowMaterial.SetColor("_EmissionColor", emissionColor * 0.5f); // The second flash is usually less intense, hence the * 0.5
                yield return new WaitForSeconds(lightningDuration / 2);

                // Return to original emission color
                windowMaterial.SetColor("_EmissionColor", originalEmissionColor);
            }
        }
    }

    void OnApplicationQuit()
    {
        ResetEmissionColor();
    }

    void OnDisable()
    {
        ResetEmissionColor();
    }

    private void ResetEmissionColor()
    {
        if (windowMaterial)
        {
            windowMaterial.SetColor("_EmissionColor", originalEmissionColor);
        }
    }
}

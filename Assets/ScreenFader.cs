using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1f;
    public bool startFadeOut = false;
    public bool startFadeIn = false;

    private bool isFading = false;

    void Start()
    {
      {
          
          if (SceneManager.GetActiveScene().name == "MainMenu") {
              fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
          }
          else if (SceneManager.GetActiveScene().name == "Hallways") {
              fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
          }

          DontDestroyOnLoad(this.gameObject);
      }

    }

    void Update()
    {
        if (startFadeOut && !isFading)
        {
            StartCoroutine(FadeOutRoutine());
            startFadeOut = false;
        }
        
        if (startFadeIn && !isFading)
        {
            StartCoroutine(FadeInRoutine());
            startFadeIn = false;
        }

    }

    public void BlackoutScreen()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
    }


    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    public IEnumerator FadeInRoutine()
{
    isFading = true;
    float elapsedTime = 0;

    while (elapsedTime < fadeSpeed)
    {
        elapsedTime += Time.deltaTime;
        float newAlpha = Mathf.Lerp(1, 0, elapsedTime / fadeSpeed);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
        yield return null;
    }

    fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
    isFading = false;
    }

    public IEnumerator FadeOutRoutine()
    {
        isFading = true;
        float elapsedTime = 0;
        Debug.Log("Starting fade out...");  // New debug line

        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(0, 1, elapsedTime / fadeSpeed);
            Debug.Log("FadeOut elapsedTime: " + elapsedTime + " newAlpha: " + newAlpha); // Modified debug line
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, newAlpha);
            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
        Debug.Log("Finished fade out");  // New debug line
        isFading = false;

        // Load the game scene after the fade out is completed
        SceneManager.LoadScene("Hallways"); 
    }

}


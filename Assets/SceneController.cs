using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public ScreenFader screenFader;

    public void StartGame() 
    {
        StartCoroutine(screenFader.FadeOutRoutine());
        SceneManager.LoadScene("Hallways");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Hallways")
        {
            screenFader.startFadeIn = true;
        }
    }
}

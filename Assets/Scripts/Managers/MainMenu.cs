using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public AudioSource backgroundMusic;
    public static bool onMainMenu = true;
    //private Animation fadeOutAnim;

    private void Start()
    {
        loadingScreen.SetActive(false);
        // fadeOutAnim = FadeOutPanel.GetComponent<Animation>();
    }
    public void PlayGame()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));

    }

    public void QuitGame()
    {
        Application.Quit();

    }

    private void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
        //backgroundMusic.enabled = false;
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        ShowLoadingScreen();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (backgroundMusic.volume > 0.01f && !operation.isDone)
        {
            backgroundMusic.volume -= Time.deltaTime / 3f;
            yield return null;
        }

        // Make sure volume is set to 0
        backgroundMusic.volume = 0;

        // Stop Music
        backgroundMusic.Stop();

    }
}

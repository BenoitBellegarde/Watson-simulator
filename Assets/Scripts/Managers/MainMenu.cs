using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool onMainMenu = true;

    public GameObject Player;
    public GameObject HudElements;
    public GameObject PauseMenu;
    public GameObject CameraContainer;
    public Camera MainCamera;

    private Animator PlayerAnimator;

    private Animation fadeOutAnim;

    private void Start()
    {
        //Make watson in sit cycle animation
        PlayerAnimator = Player.GetComponentInChildren<Animator>();
        PlayerAnimator.SetBool("Sit", true);
    }
    public void PlayGame()
    {
        //Disable main menu
        MainMenu.onMainMenu = false;

        //Enable HUD,Pause Menu, Camera follow/collision
        this.gameObject.SetActive(false);
        Player.GetComponent<CatMovement>().enabled = true;
        HudElements.SetActive(true);
        PauseMenu.SetActive(true);
        CameraContainer.GetComponent<CameraFollowCat>().enabled = true;
        MainCamera.gameObject.GetComponent<CameraCollision>().enabled = true;
 
        //Start fading animation
        //StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));

    }

    public void QuitGame()
    {
        Application.Quit();

    }


    // Fading animation (will use it later)
   /* IEnumerator LoadAsynchronously(int sceneIndex)
    {
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

    }*/
}

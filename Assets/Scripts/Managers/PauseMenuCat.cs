using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PauseMenuCat : MonoBehaviour
{
    public static bool paused = false;
    private DepthOfField dof;
    public GameObject menuElements;

    void Start()
    {
        PostProcessVolume volume = GameObject.FindGameObjectWithTag("PostProcessing").GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out dof);
        menuElements.SetActive(false);

    }


    public void Pause()
    {
        Time.timeScale = 0f;
        AudioListener.volume = 0f;

        dof.enabled.value = true;
        menuElements.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        paused = true;
    }


    public void Resume()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;

        dof.enabled.value = false;
        menuElements.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        paused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }

        }
    }


}

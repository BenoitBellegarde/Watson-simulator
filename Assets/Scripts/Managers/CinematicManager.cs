using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject player;
    public GameObject hudElements;
    public GameObject pauseMenu;
    public GameObject blackBars;
    public GameObject watsonCoins;
    public GameObject notification;

    private Camera camera1;
    private Camera camera2;
    private Animator animator;
    private Animator notificationAnimator;

    // Start is called before the first frame update
    void Start()
    {
        camera1 = GameObject.FindWithTag("Camera1").GetComponent<Camera>();
        camera2 = GameObject.FindWithTag("Camera2").GetComponent<Camera>();
        animator = player.GetComponentInChildren<Animator>();
        notificationAnimator = notification.GetComponent<Animator>();

        watsonCoins.SetActive(false);

        // Skip cutscenes (for dev purposes)
        SkipCutscenes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lvl1_Intro_Camera1()
    {
       // camera2.gameObject.SetActive(true);
        camera1.gameObject.SetActive(false);      
    }

    public void PlaySound(string sound)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+sound);
        audioSource.PlayOneShot(clip);

    }

    public void WakeUpWatson()
    {
        animator.SetFloat("LyingTime", 0f);
    }

    public void SitWatson()
    {
        animator.SetBool("Sit", true);
        animator.SetBool("Lie", false);
    }

    public void CinematicToGameplay()
    {
        blackBars.SetActive(false);
        camera2.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
        hudElements.SetActive(true);
        watsonCoins.SetActive(true);

        notificationAnimator.SetBool("Show", true);
    }

    public void SkipCutscenes()
    {
        camera2.gameObject.SetActive(false);
        camera1.gameObject.SetActive(false);
        animator.SetBool("Lie", false);
        CinematicToGameplay();
    }
}

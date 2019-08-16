using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CinematicManager : MonoBehaviour
{
    public static bool inCinematic = false;

    public AudioSource audioSource;
    public GameObject player;
    public GameObject hudElements;
    public GameObject pauseMenu;
    public GameObject blackBars;
    public GameObject watsonCoins;
    public NotificationManager notification;
    public GameObject objective;

    private Camera camera1;
    private Camera camera2;
    private Animation camera1Animation;
    private Animator animator;
    private Animator objectiveAnimator;
    private CharacterController characterController;
    private CatMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        camera1 = GameObject.FindWithTag("Camera1").GetComponent<Camera>();
        camera2 = GameObject.FindWithTag("Camera2").GetComponent<Camera>();
        camera1Animation = camera1.gameObject.GetComponent<Animation>();
        animator = player.GetComponentInChildren<Animator>();
        characterController = player.GetComponent<CharacterController>();
        objectiveAnimator = objective.GetComponent<Animator>();
        playerMovement = player.GetComponent<CatMovement>();

        camera1Animation.Play();

        watsonCoins.SetActive(false);
        playerMovement.enabled = false;

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

    public IEnumerator Lvl1_Presentation()
    {
        inCinematic = true;
        blackBars.SetActive(true);
        animator.SetBool("Sit", true);
        playerMovement.enabled = false;
        camera1.transform.position = new Vector3(3.998f,3.719f,0.36f);
        camera1.transform.Rotate(new Vector3(0f, -60f, 20f),Space.World);
        camera1.gameObject.SetActive(true);

        notification.ShowNotification();
        yield return new WaitForSeconds(5);
        camera1.transform.position = new Vector3(3.96f, 2.7f, -1.36f);
        notification.SetText("Recupere les Watson Pieces pour personnaliser Watson");
        yield return new WaitForSeconds(5);

        //....

        inCinematic = false;
        notification.HideNotification();
        blackBars.SetActive(false);
        playerMovement.enabled = true;
        camera1.gameObject.SetActive(false);
        
        yield return null;
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
        playerMovement.enabled = true;
        blackBars.SetActive(false);
        camera2.gameObject.SetActive(false);
        pauseMenu.SetActive(true);
        hudElements.SetActive(true);
        watsonCoins.SetActive(true);

        objectiveAnimator.SetBool("Show", true);
    }

    public void SkipCutscenes()
    {
        camera2.gameObject.SetActive(false);
        camera1.gameObject.SetActive(false);
        animator.SetBool("Lie", false);
        CinematicToGameplay();
    }
}

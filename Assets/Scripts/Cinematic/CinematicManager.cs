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
    public ObjectiveManager objective;

    private Camera camera1;
    private Camera camera2;
    private Animation camera1Animation;
    private Animator animator;
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
        playerMovement = player.GetComponent<CatMovement>();

        camera1Animation.Play();

        watsonCoins.SetActive(false);
        SetCinematicState("on");

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
        animator.SetBool("Sit", true);
        objective.HideObjective();
        SetCinematicState("on");

        // Set cinematic camera pos/rot
        camera1.transform.position = new Vector3(3.998f,3.719f,0.36f);
        camera1.transform.Rotate(new Vector3(0f, -60f, 20f),Space.World);
        camera1.gameObject.SetActive(true);

        //Show notification
        notification.ShowNotification();
        yield return new WaitForSeconds(5);
        camera1.transform.position = new Vector3(3.96f, 2.7f, -1.36f);
        notification.SetText("Recupere les Watson Pieces pour personnaliser Watson");

        yield return new WaitForSeconds(5);

        //....

        SetCinematicState("off");
        notification.HideNotification();
        camera1.gameObject.SetActive(false);

        //Show new objective
        objective.SetText("Fais 5 betises");
        objective.ShowObjective();

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
        SetCinematicState("off");
        camera2.gameObject.SetActive(false);
        watsonCoins.SetActive(true);

        objective.ShowObjective();
    }

    public void SkipCutscenes()
    {
        camera2.gameObject.SetActive(false);
        camera1.gameObject.SetActive(false);
        animator.SetBool("Lie", false);
        CinematicToGameplay();
    }

    public void SetCinematicState(string state)
    {
        if(state == "on")
        {
            inCinematic = true;
            hudElements.SetActive(false);
            blackBars.SetActive(true);
            playerMovement.enabled = false;
        }
        else if(state == "off")
        {
            inCinematic = false;
            blackBars.SetActive(false);
            hudElements.SetActive(true);
            playerMovement.enabled = true;
            pauseMenu.SetActive(true);
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

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
    public Camera mainCamera;
    public PlayableDirector cutscenePlayable;
    public int frameToSkip;


    private Animator animator;
    private CharacterController characterController;
    private MovementInput playerMovement;


    // Start is called before the first frame update
    void Start()
    {

        animator = player.GetComponentInChildren<Animator>();
        characterController = player.GetComponent<CharacterController>();
        playerMovement = player.GetComponent<MovementInput>();

        //camera1Animation.Play();

        watsonCoins.SetActive(false);
        SetCinematicState("on");

        // Skip cutscenes (for dev purposes)
        SkipCutscenes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator Lvl1_Presentation()
    {
        animator.SetBool("Sit", true);
        objective.HideObjective();
        SetCinematicState("on");

        // Set cinematic camera pos/rot
      /*  camera1.transform.position = new Vector3(3.998f,3.719f,0.36f);
        camera1.transform.Rotate(new Vector3(0f, -60f, 20f),Space.World);
        camera1.enabled = true;*/
        notification.SetText("Bienvenue dans le monde de Watson");
        notification.ShowNotification();
        yield return new WaitForSeconds(5);

        //Objective angle
      /*  camera1.transform.position = new Vector3(2.85f, 3.25f, 2.88f);
        camera1.transform.Rotate(new Vector3(0f, -32f, 10f), Space.Self);*/
        notification.SetText("Prends un malin plaisir a faire un maximum de betises tant que tu es tout seul");
        yield return new WaitForSeconds(5);

        //Show notification
      /*  camera1.transform.position = new Vector3(3.96f, 2.7f, -1.36f);
        camera1.transform.Rotate(new Vector3(0f, 32f, -10f), Space.Self);*/
        notification.SetText("N'oublie pas de recuperer des Watson Pieces pour personnaliser Watson plus tard");
        yield return new WaitForSeconds(5);

        SetCinematicState("off");
       // camera1.enabled = false;

        //Show new objective
        objective.SetText("Fais 5 betises");
        objective.ShowObjective();
        objective.ShowObjectiveCount();
        notification.SetText("Appuie sur " + notification.GetInputIcon("Jump") + " pour sauter");

        yield return new WaitForSeconds(4);
        notification.HideNotification();

        yield return null;
    }

    public IEnumerator Lvl1_Obj1_Advice()
    {
        notification.SetText("Cette bouteille est prete a tomber par terre...");
        notification.ShowNotification();
        yield return null;
    }
    public IEnumerator Lvl1_Obj2_Advice()
    {
        notification.SetText("Ce clavier ferait un excellent coussin... \n Appuie sur "+notification.GetInputIcon("Sit")+" pour t'asseoir");
        notification.ShowNotification();
        yield return null;
    }

    public IEnumerator Lvl1_Obj3_Advice()
    {
        notification.SetText("La litiere est evidemment beaucoup trop loin...  \n Appuie sur " + notification.GetInputIcon("Sit") + " pour te mettre en position et " + notification.GetInputIcon("Fart") + " pour te soulager",28f);
        notification.ShowNotification();
        yield return null;
    }

    public IEnumerator Lvl1_Obj4_Advice()
    {
        notification.SetText("Ce canape est beaucoup trop propre... \n Appuie sur " + notification.GetInputIcon("Attack") + " pour griffer");
        notification.ShowNotification();
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
        cutscenePlayable.time = frameToSkip;
       // camera2.enabled = false;
        mainCamera.enabled = true;
        watsonCoins.SetActive(true);

        objective.ShowObjective();
       // notification.SetText("On dirait bien que tu es tout seul pour un moment...");
       // notification.ShowNotification();
    }

    public void SkipCutscenes()
    {
      /*  camera3.enabled = false;
        camera3.gameObject.GetComponent<Animation>().Stop();
        camera2.enabled = false;
        camera1.enabled = false;*/
        animator.SetBool("Lie", false);
        CinematicToGameplay();
    }

    public void SetCinematicState(string state)
    {
        if(state == "on")
        {
            inCinematic = true;
            hudElements.SetActive(false);
            playerMovement.enabled = false;
        }
        else if(state == "off")
        {
            inCinematic = false;
            hudElements.SetActive(true);
            playerMovement.enabled = true;
            pauseMenu.SetActive(true);
        }
    }


}

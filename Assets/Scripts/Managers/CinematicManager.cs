using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    public AudioSource audioSource;
    private Camera camera1;
    private Camera camera2;
    // Start is called before the first frame update
    void Start()
    {
        camera1 = GameObject.FindWithTag("Camera1").GetComponent<Camera>();
        camera2 = GameObject.FindWithTag("Camera2").GetComponent<Camera>();
       // camera2.gameObject.SetActive(false);
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

    public void PlaySnoringSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/snoring");
        audioSource.PlayOneShot(clip);

    }
}

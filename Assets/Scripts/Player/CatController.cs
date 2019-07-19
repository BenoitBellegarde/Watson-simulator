using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatController : MonoBehaviour
{
    private int count;
    public TextMeshProUGUI countText;
    public Image Obj1Complete;
    public Image Obj2Complete;
    public AudioClip winningSound;
    private Animator animator;
    private AudioSource audioSource;
    private bool gameEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        SetCountText();
        animator = GetComponentInChildren<Animator>();
        Obj1Complete.enabled = false;
        Obj2Complete.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Obj1Complete.enabled == true && Obj2Complete.enabled == true && !gameEnded && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(winningSound,0.4f);
            gameEnded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WatsonCoin")){

            //Play collected sound
            var soundCollected = other.gameObject.GetComponent<AudioSource>();
            soundCollected.Play();
            
            //Disable collider,renderer and light during sound played
            var coinRenderer = other.gameObject.GetComponent<Renderer>();
            var coinCollider = other.gameObject.GetComponent<Collider>();
            var coinLight = other.gameObject.GetComponentInChildren<Light>();
            coinRenderer.enabled = false;
            coinCollider.enabled = false;
            coinLight.enabled = false;

            //Destroy coin after sound ended
            Destroy(other.gameObject,3f);

            //Update coin score
            count++;
            SetCountText();

            // If 10 coins collected => Set Obj1 completed
            if(Obj1Complete.enabled == false && count == 10)
            {
                Obj1Complete.enabled = true;
            }


        }
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //If cat is sit on table => Obj2 completed
        if (Obj2Complete.enabled == false && hit.gameObject.CompareTag("Table") && animator.GetBool("Sit") == true)
        {
            Obj2Complete.enabled = true;
        }
    }


    void SetCountText()
    {
        countText.SetText(count.ToString());
    }
}

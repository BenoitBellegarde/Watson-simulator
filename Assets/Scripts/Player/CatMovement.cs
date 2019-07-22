using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public AudioClip fartSound;

    // Drag & Drop the camera in this field, in the inspector
    public Transform cameraTransform;
    public string defaultPosition;

    private AudioSource audioSource;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;
    private CharacterController controller;
    private ParticleSystem fartParticle;
    private float sittingTime = 0f;

    private bool isGoingBackward = false;

    private String[] randomAnimations = { "Wash", "Lie" };

    private String sitActiveAnimation = "";
        
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        fartParticle = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        ManageDefaultPosition();
        
    }

    void Update()
    {
        if (animator.GetBool("Sit") == true)
        {
            sittingTime += Time.deltaTime;
            animator.SetFloat("SittingTime", sittingTime);

            if(sittingTime > 8)
            {
                sittingTime = 0;
                SelectRandomSitAnimation();
            }
        }


        //Sit animation
        if (Input.GetButton("Fire2"))
        {   
            if (animator.GetBool("Sit") == false)
            {
                animator.SetBool("Sit", true);
            }
        }
        else if (Input.GetButton("Fire3"))
        {
            if (!fartParticle.isPlaying)
            {
                fartParticle.Play();
                audioSource.PlayOneShot(fartSound, 0.4f);
            }
            
        }

        //Movement + jump control/animation
        if (controller.isGrounded)
        {
            animator.SetBool("isGrounded", true);
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (moveDirection.z < 0)
            {
                isGoingBackward = true;
            }
            else
            {
                isGoingBackward = false;
            }
            moveDirection = cameraTransform.TransformDirection(moveDirection);

            var altSpeed = speed;

            //Backward speed divided by 2 
            if (isGoingBackward)
            {
                altSpeed = speed/2;
            }
            else if (Input.GetButton("Run"))
            {
                
                altSpeed = speed * 1.5f;
            }
            moveDirection *= altSpeed;

            //Jump handler
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Jump");
                moveDirection.y = jumpSpeed;
            }

            //Add gravity to make it grounded
            else
            {
                moveDirection.y = -gravity * Time.deltaTime;
            }
                
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        moveDirection.y -= gravity * Time.deltaTime;
  
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float magnitudeMovement = horizontalVelocity.magnitude;
        if (isGoingBackward)
        {
            magnitudeMovement = -horizontalVelocity.magnitude;
            
        }
        animator.SetFloat("Speed", magnitudeMovement);

        controller.Move(moveDirection * Time.deltaTime);
        
        if (horizontalVelocity.magnitude > 0)
        {
            if (animator.GetBool("Sit") == true)
            {
                animator.SetBool("Sit", false);
                sittingTime = 0;
            }
            Quaternion newRotation = cameraTransform.rotation;
            newRotation.x = transform.rotation.x;
            newRotation.z = transform.rotation.z;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
    }

    private void SelectRandomSitAnimation()
    {
        /*  if(sitActiveAnimation != "")
          {
              animator.SetBool(sitActiveAnimation, false);
          }

          System.Random rnd = new System.Random();
          int rndNumber = rnd.Next(0, 2);
          sitActiveAnimation = randomAnimations[rndNumber];*/
        sitActiveAnimation = "Wash";
        animator.SetBool(sitActiveAnimation, true);
    }

    private void ManageDefaultPosition()
    {
        if (defaultPosition == "Sit")
        {
            animator.SetBool("Sit", true);
            SelectRandomSitAnimation();
        }
        else if (defaultPosition == "Lie")
        {
            animator.SetBool("Lie", true);
        }
        else if (defaultPosition == "Sleep")
        {
            animator.SetBool("Lie", true);
            animator.SetFloat("LyingTime", 11f);
        }
    }

}

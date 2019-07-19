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

    private AudioSource audioSource;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;
    private CharacterController controller;
    private ParticleSystem fartParticle;
    private float sittingTime = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        fartParticle = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
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
            }
        }

        // Block movement inputs if on main menu
        if (MainMenu.onMainMenu)
        {
            return;
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
            
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Jump");
                moveDirection.y = jumpSpeed;
            }
                
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }

        moveDirection.y -= gravity * Time.deltaTime;
  
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        animator.SetFloat("Speed", horizontalVelocity.magnitude);
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

}

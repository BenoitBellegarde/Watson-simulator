using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour
{

    public float InputX, InputZ;
    public Vector3 desiredMoveDirection;
    public bool blockRotationPlayer;
    public float desiredRotationSpeed;
    public Animator anim;
    public float speed;
    public float allowPlayerRotation;
    public Camera cam;
    public CharacterController controller;
    public bool isGrounded;

    public float jumpSpeed = 3.5f;
    public float gravity = 8f;
    public AudioClip fartSound;

    public string defaultPosition;

    private float verticalVel;
    private Vector3 moveVector;

    private AudioSource audioSource;
    private ParticleSystem fartParticle;
    private float sittingTime = 0f;
    private Vector3 moveDirection = Vector3.zero;
    private float altSpeed = 0f;

    private string sitActiveAnimation = "";
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        fartParticle = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        ManageDefaultPosition();
    }

    // Update is called once per frame
    void Update()
    {
        float lastInputX = InputX;
        float lastInputZ = InputZ;

        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        //InputX = Mathf.Lerp(lastInputX, InputX, 0.05f);
        //InputZ = Mathf.Lerp(lastInputZ, InputZ, 0.05f);

        if (anim.GetBool("Sit") == true)
        {
            sittingTime += Time.deltaTime;
            anim.SetFloat("SittingTime", sittingTime);

            if (sittingTime > 8)
            {
                sittingTime = 0;
                sitActiveAnimation = "Wash";
                anim.SetBool(sitActiveAnimation, true);
            }
        }


        //Sit animation
        if (Input.GetButton("Sit"))
        {
            if (anim.GetBool("Sit") == false)
            {
                anim.SetBool("Sit", true);
            }
        }
        else if (Input.GetButton("Fart"))
        {
            if (!fartParticle.isPlaying)
            {
                fartParticle.Play();
                audioSource.PlayOneShot(fartSound, 0.4f);
            }

        }
        else if (Input.GetButtonDown("Attack"))
        {
            anim.SetTrigger("Attack");

        }

        //Stay the character grounded
        isGrounded = controller.isGrounded;
        altSpeed = speed;
        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);

            if (Input.GetButton("Run"))
            {

                altSpeed = speed * 1.5f;
            }
            moveDirection *= altSpeed;

            //Jump handler
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("Jump");
                moveDirection.y = jumpSpeed;
            }
            //Add gravity to make it grounded
            else
            {
                moveDirection.y = -gravity * Time.deltaTime;
            }
            //verticalVel -= 0;
        }
        else
        {
            anim.SetBool("isGrounded", false);
            //verticalVel -= 2;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        desiredMoveDirection = forward * InputZ + right * InputX;
      //  moveDirection.x = desiredMoveDirection.x;
       // moveDirection.z = desiredMoveDirection.z;
      //  controller.Move(moveDirection * Time.deltaTime);

        InputMagnitude(InputX, InputZ);
    }

    void PlayerMoveAndRotation(float InputX, float InputZ,float speed)
    {

      /*  Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        desiredMoveDirection = forward * InputZ + right * InputX;
        moveDirection.x = desiredMoveDirection.x;
        moveDirection.z = desiredMoveDirection.z;
        controller.Move(moveDirection * altSpeed * Time.deltaTime);*/
        if (blockRotationPlayer == false && desiredMoveDirection != Vector3.zero)
        {
            if (anim.GetBool("Sit") == true)
            {
                anim.SetBool("Sit", false);
                sittingTime = 0;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), desiredRotationSpeed);
        }
    }

    void InputMagnitude(float InputX, float InputZ)
    {
        //Calculate Input Vectors

        anim.SetFloat("InputZ", InputZ, 0.0f, Time.deltaTime * 2f);
        anim.SetFloat("InputX", InputX, 0.0f, Time.deltaTime * 2f);


        //Calculate Input Magnitude
        speed = new Vector2(InputX, InputZ).sqrMagnitude;
        
      /*  if (speed <= 0)
        {
            speed = 0f;
        }*/
        anim.SetFloat("InputMagnitude", speed, 0.0f, Time.deltaTime);
        //Physically move Player
        if (speed > allowPlayerRotation)
        {
            PlayerMoveAndRotation(InputX, InputZ,speed);
        }

    }

    private void ManageDefaultPosition()
    {
        if (defaultPosition == "Sit")
        {
            anim.SetBool("Lie", false);
            anim.SetBool("Sit", true);

            //SelectRandomSitAnimation();
        }
        else if (defaultPosition == "Lie")
        {
            anim.SetBool("Lie", true);
        }
        else if (defaultPosition == "Sleep")
        {
            anim.SetBool("Lie", true);
            anim.SetFloat("LyingTime", 11f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCatCutscene : MonoBehaviour
{
    public Vector3 target;
    public float rotationY;
    public float moveSpeed = 6.0f;
    public GameObject catToMove;
    private CatMovement catMovement;
    private Animator catAnimator;

    // Start is called before the first frame update
    void Start()
    {
        catMovement = catToMove.GetComponent<CatMovement>();
        catAnimator = catToMove.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget(target);
        
    }

    void MoveTowardsTarget(Vector3 target)
    {
        //var cc = GetComponent<CharacterController>();
        Vector3 offset = target - catToMove.transform.position;
       // Debug.Log(target);
       // Debug.Log(catToMove.transform.position);

        //Get the difference.
        if (offset.magnitude > .1f)
        {
           // Debug.Log(offset.magnitude);
            //If we're further away than .1 unit, move towards the target.
            //The minimum allowable tolerance varies with the speed of the object and the framerate. 
            // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
            offset = offset.normalized * moveSpeed;
            float speedAnimator = moveSpeed;
            if(speedAnimator > 1.5f)
            {
                speedAnimator = 1.5f;
            }
            //normalize it and account for movement speed.
            catMovement.Move(offset,rotationY, speedAnimator);
            //actually move the character.
        }
        else
        {
            catAnimator.SetFloat("Speed", 0f);
            catAnimator.SetBool("Sit", true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveArrow : MonoBehaviour
{
    public Vector3 position1 = new Vector3(0.0f, 41.5f, 0.0f);
    public Vector3 position2 = new Vector3(0.0f, 51.5f, 0.0f);
    public float speed = 15f;

    private Vector3 currentTargetDestination;

    public float distanceTolerance = 0.5f; //you can change the tolerance to whatever you need it to be

    void Start()
    {
        transform.localPosition = position1; //set the initial position
        currentTargetDestination = position2;
    }

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTargetDestination, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, currentTargetDestination) <= distanceTolerance)
        {
            //once we reach the current destination, set the other location as our new destination
            if (currentTargetDestination == position1)
            {
                currentTargetDestination = position2;
            }
            else
            {
                currentTargetDestination = position1;
            }
        }
    }

}

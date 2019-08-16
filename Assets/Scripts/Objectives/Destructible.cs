using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;
    public ObjectiveManager objectiveManager;
    public bool countAsObjective = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
            if (countAsObjective)
            {
                objectiveManager.IncrementObjectiveCount();
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitObjectiveManager : MonoBehaviour
{
    public ObjectiveManager objManager;
    public GameObject targetObject = null;
    public GameObject newTargetObject = null;

    private Animator animatorPlayer;
    private ObjectiveArrow objArrow;
    // Start is called before the first frame update
    void Start()
    {
        animatorPlayer = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
        objArrow = GetComponentInChildren<ObjectiveArrow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && objArrow != null)
        {
            if (animatorPlayer.GetBool("Sit") == true)
            {
                objManager.IncrementObjectiveCount();
                if (targetObject != null && newTargetObject != null)
                {
                    targetObject.SetActive(false);
                    newTargetObject.SetActive(true);
                    Destroy(objArrow.gameObject);
 
                }
            }
            
            
        }

    }
}

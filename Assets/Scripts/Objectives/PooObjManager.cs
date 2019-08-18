using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooObjManager : MonoBehaviour
{
    private ObjectiveArrow objArrow;
    public GameObject pooObj;
    public ObjectiveManager objManager;

    private Animator animatorPlayer;
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
            Debug.Log(animatorPlayer.GetBool("Sit"));
            
            if (animatorPlayer.GetBool("Sit") == true && Input.GetButton("Fire3"))
            {
                Destroy(objArrow.gameObject);
                objManager.IncrementObjectiveCount();
                pooObj.transform.position = new Vector3(transform.position.x, pooObj.transform.position.y, transform.position.z-0.10f);
                Instantiate(pooObj, pooObj.transform.position, pooObj.transform.rotation);
            }


        }

    }
}

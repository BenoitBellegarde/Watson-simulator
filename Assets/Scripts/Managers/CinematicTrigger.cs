using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    public string cinematicName;
    public CinematicManager cinematicManager; 

    private Dictionary<string, bool> cinematicDone = new Dictionary<string, bool>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!cinematicDone.ContainsKey(cinematicName) || cinematicDone[cinematicName] != true)
        {
            switch (cinematicName)
            {
                case "Lvl1_Presentation":
                    cinematicDone[cinematicName] = true;
                    StartCoroutine(cinematicManager.Lvl1_Presentation());
                    break;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
    public GameObject emote;
    public Camera cameraTarget;


    // Update is called once per frame
    void Update()
    {
        Vector3 namePose = cameraTarget.WorldToScreenPoint(this.transform.position);
        emote.transform.position = namePose;
    }
}

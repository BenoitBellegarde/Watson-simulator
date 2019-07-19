using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDTimer : MonoBehaviour
{
    private TextMeshProUGUI startText; // used for showing countdown from 3, 2, 1 
    private float countdown = 600f;

    private void Start()
    {
        startText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Debug.Log(countdown);
        if(countdown > 1)
        {
            countdown -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(countdown / 60F);
            int seconds = Mathf.FloorToInt(countdown - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            startText.text = niceTime;
        }
        
        
    }
}

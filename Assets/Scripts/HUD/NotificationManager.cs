using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class NotificationManager : MonoBehaviour
{
    protected Animator animator;
    protected TextMeshProUGUI text;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideNotification()
    {
 
        if (!CinematicManager.inCinematic && animator.GetBool("isShown"))
        {
            animator.SetBool("Show", false);
            animator.SetBool("isShown", false);
        }
        
    }


    public void ShowNotification()
    {
        if (!animator.GetBool("isShown"))
        {
            animator.SetBool("Show", true);
            animator.SetBool("isShown", true);
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public TextMeshProUGUI GetTextComponent()
    {
        return text;
    }

    public void SetText(string newText)
    {
        text.SetText(newText);
    }

    public string GetInputIcon(string input)
    {
        String[] controllers = Input.GetJoystickNames();
        String nameIcon = "";
        Debug.Log(controllers.Length);
        switch (input)
        {
            case "Jump":
                if(controllers.Length > 0)
                {
                    nameIcon = "<sprite=\"360_A\" index=0>";
                    
                }
                else
                {
                    //nameIcon = "<sprite=\"Keyboard_Space\" index=0>";
                    nameIcon = "Espace";

                }
                break;
                
        }
        return nameIcon;
    }
}


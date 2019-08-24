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

    private Queue<NotificationManager> queueNotification = new Queue<NotificationManager>();

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
            if (queueNotification.Count > 0)
            {
                NotificationManager newNotification = queueNotification.Dequeue();
                SetText(newNotification.GetTextComponent().text);
                ShowNotification(true);
            }
            else
            {
                animator.SetBool("Show", false);
                animator.SetBool("isShown", false);
            }
              
        }
        
    }



    public void ShowNotification(bool fromDequeue = false)
    {
        if (fromDequeue)
        {
            Invoke("HideNotification", 4f);
        }
        else if (!animator.GetBool("isShown"))
        {
            animator.SetBool("Show", true);
            animator.SetBool("isShown", true);

           
            Invoke("HideNotification", 4f);
        }
        else
        {
            queueNotification.Enqueue(this);
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

    public void SetText(string newText, float textSize=32f)
    {       
        text.fontSize = textSize;
        text.SetText(newText);
    }

    public string GetInputIcon(string input)
    {
        String[] controllers = Input.GetJoystickNames();
        String nameIcon = "";
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

            case "Sit":
                if (controllers.Length > 0)
                {
                    nameIcon = "<sprite=\"360_B\" index=0>";

                }
                else
                {
                    //nameIcon = "<sprite=\"Keyboard_Space\" index=0>";
                    nameIcon = "Clic droit";

                }
                break;

            case "Fart":
                if (controllers.Length > 0)
                {
                    nameIcon = "<sprite=\"360_Y\" index=0>";

                }
                else
                {
                    //nameIcon = "<sprite=\"Keyboard_Space\" index=0>";
                    nameIcon = "Clic molette";

                }
                break;
            case "Attack":
                if (controllers.Length > 0)
                {
                    nameIcon = "<sprite=\"360_X\" index=0>";

                }
                else
                {
                    //nameIcon = "<sprite=\"Keyboard_Space\" index=0>";
                    nameIcon = "Clic gauche";

                }
                break;

        }
        return nameIcon;
    }
}


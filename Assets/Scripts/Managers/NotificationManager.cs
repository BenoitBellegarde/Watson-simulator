using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideNotification()
    {
        animator.SetBool("Show", false);
    }

    public void ShowNotification()
    {
        animator.SetBool("Show", true);
    }
}


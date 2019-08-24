using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideHUD()
    {
        animator.SetBool("Show", false);
    }

    public void ShowHUD()
    {
        animator.SetBool("Show", true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    protected Animator animator;
    protected TextMeshProUGUI text;

    void Awake()
    {
        animator = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideObjective()
    {
        if (animator.GetBool("isShown"))
        {
            animator.SetBool("Show", false);
            animator.SetBool("isShown", false);
        }
    }

    public void ShowObjective()
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
}

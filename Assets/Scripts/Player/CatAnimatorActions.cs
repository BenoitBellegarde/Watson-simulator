using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimatorActions : MonoBehaviour
{
    private Animator catAnimator;

    private void Start()
    {
        catAnimator = GetComponent<Animator>();
    }
    public void SetAttackingBool(string state)
    {
        if (state == "true")
        {
            catAnimator.SetBool("isAttacking", true);
        }
        else
        {
            catAnimator.SetBool("isAttacking", false);
        }
    }
}

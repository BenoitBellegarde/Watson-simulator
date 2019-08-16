using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject objectiveCount;

    protected Animator animator;
    protected TextMeshProUGUI text;
    protected TextMeshProUGUI countText;

    public int maxObjectiveCount = 5;
    public static int countObjective = 0;

    void Awake()
    {
        animator = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        countText = objectiveCount.GetComponentInChildren<TextMeshProUGUI>();
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

    public GameObject GetObjectiveCount()
    {
        return objectiveCount;
    }

    public TextMeshProUGUI getObjectiveCountText()
    {
        return countText;
    }

    public void SetText(string newText)
    {
        text.SetText(newText);
    }

    public void ShowObjectiveCount()
    {
        objectiveCount.SetActive(true);
    }
    public void HideObjectiveCount()
    {
        objectiveCount.SetActive(false);
    }
    public void IncrementObjectiveCount()
    {
        countObjective++;
        countText.SetText(countObjective + "/"+ maxObjectiveCount);
    }
}

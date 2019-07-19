using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_Animator_CatShort : MonoBehaviour {

    Animator anim;
    int i = 0;
    public int size = 10;
    public bool rot = false;
    Vector3 startpose;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        startpose = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (rot == true)
        {
            transform.Rotate(0f, 50f*Time.deltaTime, 0f);
        }
        anim.SetInteger("number", i);
	}
    public void NextAnimation()
    {
        if (i > size)
        {
            i = 0;
        }
        i++;
        transform.position = startpose;
    }
    public void BackAnimation()
    {
        i--;
        if (i < 0)
        {
            i = size;
        }
        transform.position = startpose;
    }
}

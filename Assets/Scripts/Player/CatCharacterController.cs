using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCharacterController : MonoBehaviour
{
    public float speed;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0.0f, v) * speed;

        //transform.Translate(movement,Space.Self);
        rb.AddForce(movement);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController control;
    public float speed = 10f;

    public float gravity = -9.8f;
    Vector3 velocity;

    public Transform groundCheck;
    public float groundDist = 0.3f;
    public LayerMask groundMask;
    public bool isGrounded;
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if(isGrounded && velocity.y<0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        control.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        control.Move(velocity * Time.deltaTime);
    }
}

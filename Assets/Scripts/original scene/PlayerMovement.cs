using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 7;
    public float sensitivity = 5;
    private float xMovement;
    private float zMovement;
    private float mouseX;
    private float mouseY;
    public Camera mainCmera;
    public float jumpVelocity = 100;
    public Rigidbody rb;
    public float timer = 2;
    float t;

    public Pickup pickupReference;

    public void Start()
    {
        pickupReference = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Pickup>();
        Cursor.lockState = CursorLockMode.Locked;
        t = timer;
    }

    public void Update()
    {
        if (pickupReference.isInteractedWithDoor == false)
        {
            xMovement = Input.GetAxis("Horizontal");
            zMovement = Input.GetAxis("Vertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
        else if (pickupReference.isInteractedWithDoor == true)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }
    }

    public void FixedUpdate()
    {
        Vector3 movement = new Vector3(xMovement, 0f, zMovement) * speed * Time.deltaTime;
        transform.position += transform.rotation * movement;
        transform.Rotate(0, mouseX * sensitivity, 0);
        mainCmera.transform.Rotate(-mouseY * sensitivity, 0, 0);
    }
}

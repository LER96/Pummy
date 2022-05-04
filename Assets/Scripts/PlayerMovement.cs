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

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        xMovement = Input.GetAxis("Horizontal");
        zMovement = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    public void FixedUpdate()
    {
        Vector3 movement = new Vector3(xMovement, 0f, zMovement) * speed * Time.deltaTime;
        transform.position += transform.rotation * movement;
        transform.Rotate(0, mouseX * sensitivity, 0);
        mainCmera.transform.Rotate(-mouseY * sensitivity, 0, 0);
    }
}

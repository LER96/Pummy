using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{
    float mouseX;
    float mouseY;
    public float sensitivity = 5;
    public Transform body;

    float xrotation;
    [SerializeField] public OpeningDoors doorReference;

    // Start is called before the first frame update
    void Start()
    {
        doorReference = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OpeningDoors>();
        //Cursor.lockState = CursorLockMode.Locked;
        transform.SetParent(body);
    }

    // Update is called once per frame
    void Update()
    {
        if (doorReference.isInteractedWithDoor == false)
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            xrotation -= mouseY;
            xrotation = Mathf.Clamp(xrotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xrotation, 0, 0);
            body.Rotate(Vector3.up * mouseX);
        }
    }
}

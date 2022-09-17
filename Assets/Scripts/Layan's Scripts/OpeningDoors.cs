using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningDoors : MonoBehaviour
{
    [Header("Pickup Settings")]
    public bool isInteractedWithDoor = false;
    [SerializeField] Transform holdArea;
    [SerializeField] RaycastHit hit;
    private Rigidbody _heldItem;
    private GameObject _holdObject;
    float yRot = 0;
    public float ySensitivity = 300f;

    [Header("Physics")]
    [SerializeField] private float _pickupRange = 5f;
    [SerializeField] private float _pickupForce = 150f;

    [SerializeField] Image middlePoint;
    [SerializeField] Camera mainCamera;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_holdObject == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _pickupRange))
                {
                    HoldDoor(hit.transform.gameObject);
                }
            }
            else
            {
                LeaveDoor();
            }
        }

        if (_holdObject != null)
        {
            OpenDoor();
        }

        ChangePointColor();
    }

    void HoldDoor(GameObject obj)
    {
        
        if (obj.gameObject.CompareTag("Door"))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            _heldItem = obj.GetComponent<Rigidbody>();
            isInteractedWithDoor = true;
            _heldItem.useGravity = false;
            _heldItem.drag = 10;
            _holdObject = obj;
        }
    }

    void LeaveDoor()
    {
        isInteractedWithDoor = false;
        _heldItem.useGravity = true;
        _heldItem.drag = 1;
        _heldItem.constraints = RigidbodyConstraints.None;
        _heldItem.transform.parent = null;
        _holdObject = null;
    }

    void OpenDoor()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Vector3.Distance(_holdObject.transform.position, holdArea.position) > 0.1f && Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            holdArea.position = raycastHit.point;
            Vector3 moveDirection = (holdArea.position - _holdObject.transform.position);
            _heldItem.AddForce(moveDirection * _pickupForce);
        }
    }

    void ChangePointColor()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _pickupRange))
        {
            middlePoint.color = Color.red;
           
        }
        else
        {
            middlePoint.color = Color.white;
        }
    }

}

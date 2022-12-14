using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarryItems : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask lockerMask;
    [SerializeField] LockCode _lock;
    private Rigidbody _heldItem;
    private GameObject _holdObject;
    float yRot = 0;
    public float ySensitivity = 300f;

    [Header("Physics")]
    [SerializeField] private float _pickupRange;
    [SerializeField] private float _pickupForce = 150f;
    [SerializeField] Image middlePoint;

    private void Update()
    {
        CarryingItems();
    }

    public void CarryingItems()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_holdObject == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _pickupRange))
                {
                    PickupItems(hit.transform.gameObject);
                }
            }
            else
            {
                DropObjects();
            }

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _pickupRange, lockerMask))
            {
                _lock.IsLockPressed = true;
                Debug.Log("pressed");
            }
        }

        if (_holdObject != null)
        {
            MoveObjects();
        }
        ChangePointColor();
    }

    void PickupItems(GameObject obj)
    {
        //can change to compare tag
        if (obj.gameObject.CompareTag("CanGrab"))
        {
            _heldItem = obj.GetComponent<Rigidbody>();
            _heldItem.useGravity = false;
            _heldItem.drag = 10;
            _heldItem.constraints = RigidbodyConstraints.FreezeRotation;
            _heldItem.transform.parent = holdArea;
            _holdObject = obj;
        }
    }

    void DropObjects()
    {
        _heldItem.useGravity = true;
        _heldItem.drag = 1;
        _heldItem.constraints = RigidbodyConstraints.None;
        _heldItem.transform.parent = null;
        _holdObject = null;
    }

    void MoveObjects()
    {
        if (Vector3.Distance(_holdObject.transform.position, holdArea.position) > 0.1f)
        {
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

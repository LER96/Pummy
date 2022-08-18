using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickiupItem : MonoBehaviour
{
    [SerializeField] RaycastHit raycastHit;
    [SerializeField] private float itemPickupRange = 5f;

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycastHit, itemPickupRange)) 
        {
            if (raycastHit.transform.TryGetComponent<ItemObject>(out ItemObject item) && Input.GetKeyDown(KeyCode.E))
            {
                item.HandlePickupItem();
            }
        }
    }
}

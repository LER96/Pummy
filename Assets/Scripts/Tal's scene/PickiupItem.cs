using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickiupItem : MonoBehaviour
{
    [SerializeField] RaycastHit hit;
    [SerializeField] private float itemPickupRange = 5f;

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, itemPickupRange)) 
        {
            if (hit.transform.TryGetComponent<ItemObject>(out ItemObject item) && Input.GetKeyDown(KeyCode.E))
            {
                item.HandlePickupItem();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRaycastTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent OnRayHit;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask DadDoor;
    [SerializeField] GameObject ExitRoomTrigger;
    bool isInTrigger = false;
    bool isInRoom = false;

    void FixedUpdate()
    {
        if (isInRoom == true)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, DadDoor) && isInTrigger == true)
            {
                OnRayHit.Raise();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InsideRoomTrigger"))
        {
            isInRoom = true;
            ExitRoomTrigger.SetActive(true);
        }

        if (other.CompareTag("ExitRoomTrigger"))
        {
            isInTrigger = true;
            Debug.Log("yes");
        }
    }
}

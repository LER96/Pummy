using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent OnRayHit;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask DadDoor;
    [SerializeField] Animation animation;
    private bool isInTrigger = false;
    [SerializeField] GameObject light;

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, DadDoor) && isInTrigger == true)
        {
            OnRayHit.Raise();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DoorTrigger"))
        {
            isInTrigger = true;
        }
    }

    public void PlayAnimation()
    {
        animation.Play();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent OnRayHit;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask DadDoor;
    [SerializeField] Animation anim;

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 5f, DadDoor))
        {
            OnRayHit.Raise();
        }
    }

    public void PlayAnimation()
    {
        anim.Play();
    }
}
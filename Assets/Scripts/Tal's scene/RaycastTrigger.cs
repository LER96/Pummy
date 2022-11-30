using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent OnRayHit;
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask DadDoor;
    [SerializeField] Animation anim;

    private void OnTriggerEnter(Collider coll)
    {
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, DadDoor))
        {
            OnRayHit.Raise();
            Debug.Log("Triggered");
        }
    }

    public void Something()
    {
        anim.Play();
    }
}
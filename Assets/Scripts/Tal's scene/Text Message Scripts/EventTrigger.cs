using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent OnPlayerDetected;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            OnPlayerDetected.Raise();
        }
    }
}

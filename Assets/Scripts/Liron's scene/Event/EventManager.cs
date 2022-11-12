using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] FieldOfView view;

    public static event Action lookEvent;
    public static event Action notLookEvent;
    void Start()
    {
        view = player.GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(view.doesSee== false)
        {
            notLookEvent?.Invoke();
        }
        else
        {
            lookEvent?.Invoke();
        }
    }
}

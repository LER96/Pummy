﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    EnemyBase currentState;
    public EnemyAI enemyPatrol = new EnemyAI();

    [Header("Patrol mode")]
    public Vector3 target;
    public NavMeshAgent agent;
    public List<Transform> waypoints;
    public GameObject[] points;
    public int indexWaypoint = 0;


    private void Start()
    {
        currentState = enemyPatrol;
        agent = GetComponent<NavMeshAgent>();
        currentState.EnterState(this);
    }
    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBase state)
    {
        currentState = state;
        state.EnterState(this);
    }

}


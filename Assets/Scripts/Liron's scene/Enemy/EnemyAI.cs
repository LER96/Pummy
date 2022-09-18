using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] List<Transform> waypoints;
    [SerializeField] GameObject[] points;
    [SerializeField] int indexWaypoint=0;
    Vector3 target;

    private void Start()
    {
        points = GameObject.FindGameObjectsWithTag("WayPoint");
        for(int i=0; i<points.Length; i++)
        {
            waypoints.Add(points[i].transform);
        }
        agent = GetComponent<NavMeshAgent>();
        NextPoint();
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position,target)<1)
        {
            Reset();
            NextPoint();
        }
    }

    public void NextPoint()
    {
        target = waypoints[indexWaypoint].position;
        agent.SetDestination(target);
    }

    public void Reset()
    {
        //indexWaypoint++;
        if (indexWaypoint < waypoints.Count)
        {
            indexWaypoint = Random.Range(0, waypoints.Count);
        }
        else
            indexWaypoint = 0;
    }

}

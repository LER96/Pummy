using UnityEngine;

public class EnemyAI: EnemyBase
{
    public override void EnterState(EnemyBehavior behavior)
    {
        behavior.points = GameObject.FindGameObjectsWithTag("WayPoint");
        for (int i = 0; i < behavior.points.Length; i++)
        {
            behavior.waypoints.Add(behavior.points[i].transform);
        }
        NextPoint(behavior);
    }
    public override void UpdateState(EnemyBehavior behavior)
    {
        if (Vector3.Distance(behavior.transform.position, behavior.target) < 1)
        {
            Reset(behavior);
            NextPoint(behavior);
        }
    }

    public void NextPoint(EnemyBehavior behavior)
    {
        behavior.target = behavior.waypoints[behavior.indexWaypoint].position;
        behavior.agent.SetDestination(behavior.target);
    }
    public void Reset(EnemyBehavior behavior)
    {
        if (behavior.indexWaypoint < behavior.waypoints.Count)
        {
            behavior.indexWaypoint = Random.Range(0, behavior.waypoints.Count);
        }
        else
            behavior.indexWaypoint = 0;
    }
}


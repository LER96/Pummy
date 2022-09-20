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
//NavMeshAgent agent;
//[SerializeField] List<Transform> waypoints;
//[SerializeField] GameObject[] points;
//[SerializeField] int indexWaypoint = 0;
//Vector3 target;

//private void Start()
//{
//    points = GameObject.FindGameObjectsWithTag("WayPoint");
//    for (int i = 0; i < points.Length; i++)
//    {
//        waypoints.Add(points[i].transform);
//    }
//    agent = GetComponent<NavMeshAgent>();
//    NextPoint();
//}
//private void Update()
//{
//    if (Vector3.Distance(transform.position, target) < 1)
//    {
//        Reset();
//        NextPoint();
//    }

//}
//public void NextPoint()
//{
//    target = waypoints[indexWaypoint].position;
//    agent.SetDestination(target);

//}
//public void Reset()
//{
//    if (indexWaypoint < waypoints.Count)
//    {
//        indexWaypoint = Random.Range(0, waypoints.Count);
//    }
//    else
//        indexWaypoint = 0;
//}

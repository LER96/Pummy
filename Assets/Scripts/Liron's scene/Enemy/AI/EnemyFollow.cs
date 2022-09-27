using UnityEngine;

public class EnemyFollow : EnemyBase
{
    public override void EnterState(EnemyBehavior behavior)
    {
        NextPoint(behavior);
    }
    public override void UpdateState(EnemyBehavior behavior)
    {
        if (Vector3.Distance(behavior.transform.position, behavior.target) < 4)
        {
            //Reset(behavior);
            Debug.Log("Game Over");
        }
    }
    public void NextPoint(EnemyBehavior behavior)
    {
        behavior.target = behavior.grapOnSight[0].position;
        behavior.agent.SetDestination(behavior.target);
    }
}

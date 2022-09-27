using UnityEngine;

public class EnemyLook : EnemyBase
{
    float range;
    public override void EnterState(EnemyBehavior behavior)
    {
        behavior.target = behavior.transform.position;
        behavior.agent.SetDestination(behavior.target);
        range = Random.Range(-20, 20);
    }
    public override void UpdateState(EnemyBehavior behavior)
    {
        behavior.head.Rotate(new Vector3(0, range, 0) * 2 *  Time.deltaTime);
    }
}

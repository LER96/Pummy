using UnityEngine;

public abstract class EnemyBase
{
    public abstract void EnterState(EnemyBehavior behavior);
    public abstract void UpdateState(EnemyBehavior behavior);
}

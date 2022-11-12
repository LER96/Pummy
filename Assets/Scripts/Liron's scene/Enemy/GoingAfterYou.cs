using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoingAfterYou : MonoBehaviour
{
    GameObject player;
    public Vector3 target;
    public NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        EventManager.notLookEvent += AfterHim;
    }

    public virtual void AfterHim()
    {
        target = player.transform.position;
        agent.SetDestination(target);
    }
}

public class NotGoing : GoingAfterYou
{
    void Start()
    {
        EventManager.lookEvent += AfterHim;
    }
    public override void AfterHim()
    {
        target = this.transform.position;
        agent.SetDestination(target);
    }

}

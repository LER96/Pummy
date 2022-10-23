using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    EnemyBase currentState;
    public EnemyAI enemyPatrol = new EnemyAI();
    public EnemyFollow enemyNotice = new EnemyFollow();

    [Header("Patrol mode")]
    public Vector3 target;
    public NavMeshAgent agent;
    public List<Transform> waypoints;
    public GameObject[] points;
    public int indexWaypoint = 0;

    [Header("Search mode")]
    public Transform head;
    public float maxTime;
    [SerializeField] float time = 0;

    [Header("Constant")]
    public List<Transform> grapOnSight = new List<Transform>();
    public bool look;

    

 

    private void Start()
    {
        currentState = enemyPatrol;
        agent = GetComponent<NavMeshAgent>();
        currentState.EnterState(this);
    }
    private void Update()
    {
        LockOn();
        currentState.UpdateState(this);
        if (time < maxTime && look==false)
        {
            time += Time.deltaTime;
        }
        else
        {
            CheckState();
            SwitchState(currentState);
            GiveNumber();
        }
    }

    public void SwitchState(EnemyBase state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void CheckState()
    {
        if (look)
        {
            currentState = enemyNotice;
            currentState.EnterState(this);
        }
        else
        {
            if (currentState == enemyPatrol)
            {
                currentState.EnterState(this);
            }
            else
            {
                currentState = enemyPatrol;
                head.localRotation = Quaternion.LookRotation(Vector3.forward);
                currentState.EnterState(this);
            }
        }
        
    }

    public void LockOn()
    {
        if (grapOnSight.Count > 0)
        {
            target = grapOnSight[0].position;
            look = true;
        }
        else
        {
            look = false;
        }
    }
    public void GiveNumber()
    {
        time = 0;
        maxTime = Random.Range(0, 11);
    }

}


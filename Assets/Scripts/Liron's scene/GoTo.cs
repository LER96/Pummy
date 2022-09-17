using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] LayerMask grapmask;
    [SerializeField] float dropDist;
    [SerializeField] float walkSpeed;
    Vector3 hookpoint;
    [SerializeField] bool isWalking;
    //bool isGrappling;

    FieldOfView fow;
    public bool ready;
    Transform target;
    public List<Transform> grapOnSight = new List<Transform>();

    private void Update()
    {
        CheckOnSight();
        LockOn();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isWalking)
            {
                ResetAll();
            }
            else
            {
                WalkTo();
            }
        }
        if(isWalking)
        {
            WalkTo();
        }
    }
    //takes the target lists from the field of view script
    void CheckOnSight()
    {
        fow = GetComponent<FieldOfView>();
        if (fow.visibleTargets.Count > 0)
        {
            //duplicate the list
            foreach (Transform onTarget in fow.visibleTargets)
            {
                if (grapOnSight.Count < fow.visibleTargets.Count)
                {
                    grapOnSight.Add(onTarget);
                }
            }
        }
        else
        {
            //delete the list
            grapOnSight.Clear();
        }
    }

    void LockOn()
    {
        //set the target icon onto the target position
        if (grapOnSight.Count > 0)
        {
            target = grapOnSight[0];
            ready = true;
        }
        else
        {
            ready = false;
        }
    }

    //check if we hit the grappling point
    void WalkTo()
    {
        isWalking=true;
        hookpoint = target.position;
        body.position = Vector3.Lerp(body.position, hookpoint, walkSpeed * Time.deltaTime);
        if (Vector3.Distance(body.position, hookpoint) < dropDist)
        {
            ResetAll();
        }
    }

    void ResetAll()
    {
        isWalking = false;
    }
}

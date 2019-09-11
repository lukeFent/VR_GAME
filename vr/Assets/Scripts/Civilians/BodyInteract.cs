using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyInteract : InteractClass
{


    bool intercepted = false;
    public float speed;
    public bool hit = false;


    public override void Interact(Vector3 direction)
    {
        base.Interact(direction);
        intercepted = true;
        hit = true;
    }

    float HeadsOrTails()
    {
        float coin = Random.Range(0f, 1f);
        if (coin < 0.5f)
            return 0f;

        return 1f;
    }

    public void HeadToSafety(Vector3 destination, float maxRange)
    {
        anim.SetTrigger("Run");
        anim.SetFloat("RunBlend", HeadsOrTails());
        anim.ResetTrigger("Cower");
        agent.isStopped = false;
        agent.SetDestination(destination);
        if (HasArrived(destination))
        {
            StartCoroutine(RunCouroutine(destination, maxRange));
        }
    }

    IEnumerator RunCouroutine(Vector3 destination, float maxRange)
    {

        //Vector3 heading = destination - transform.position;

        while (Vector3.Distance(transform.position, destination) > maxRange)
        {
            //Debug.Log(heading.sqrMagnitude);
            yield return null;
        }

        anim.SetTrigger("Cower");
        anim.ResetTrigger("Run");
        agent.isStopped = true;

    }

    public bool HasArrived(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) <= 1)
        {
            return true;
        }
        else
            return false;
    }





}

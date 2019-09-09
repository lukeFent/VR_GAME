using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh_Zomb : MonoBehaviour
{
    public BodyInteract[] bodyGuards;
    int layerMask = 1 << 9;
    public Transform raycastPoint;

    public bool kill = false;

    public ZombInteract zombie;

    // Start is called before the first frame update
    void Start()
    {

        zombie = GetComponent<ZombInteract>();
        bodyGuards = FindObjectsOfType<BodyInteract>();
        HeadToClosestBody(GetClosestBody().position);

    }



    private void FixedUpdate()
    {
        if (isBodyInFront())
            Attack();

        if (kill)
        {
            zombie.Interact(Vector3.zero);
            kill = false;
        }
    }


    void Attack()
    {
        zombie.agent.isStopped = true;
        zombie.anim.SetTrigger("Attack");
        StartCoroutine(LookAround());



    }

    bool isBodyInFront()
    {
        Vector3 fwd = raycastPoint.position + raycastPoint.forward * 0.5f;

        Debug.DrawLine(raycastPoint.position, fwd);

        return Physics.Linecast(raycastPoint.position, fwd, layerMask);

    }

    void HeadToClosestBody(Vector3 closestBody)
    {

        zombie.anim.applyRootMotion = false;


        //transform.LookAt(closestBody);

        zombie.agent.isStopped = false;
        zombie.agent.SetDestination(closestBody);
    }


    Transform GetClosestBody()
    {
        List<Transform> bodyPositions = new List<Transform>();





        for (int i = 0; i < bodyGuards.Length; i++)
        {
            if (!bodyGuards[i].hit)
            {
                bodyPositions.Add(bodyGuards[i].transform);

            }

        }

        if (bodyPositions.Count == 0)
            return null;

        Transform target = bodyPositions[GetClosestDistances(bodyPositions)];

        Debug.Log("Next target is  " + target);
        return target;

    }


    int GetClosestDistances(List<Transform> positions)
    {
        int distanceIterator = 0;

        float initialDistance = Vector3.Distance(transform.position, positions[0].position);

        for (int i = 0; i < positions.Count; i++)
        {
            if (Vector3.Distance(transform.position, positions[i].position) < initialDistance)
            {
                initialDistance = Vector3.Distance(transform.position, positions[i].position);
                distanceIterator = i;
            }


        }


        return distanceIterator;
    }


    IEnumerator LookAround()
    {
        zombie.anim.applyRootMotion = true;
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Looking around");

        if (GetClosestBody() != null)
        {
            Vector3 closestBody = GetClosestBody().position;
            zombie.anim.ResetTrigger("Attack");
            float lookAngle = Vector3.Angle(closestBody, transform.forward);
            zombie.anim.SetFloat("Direction", -lookAngle);
            Debug.Log("Look angle is " + lookAngle);
            StartCoroutine(WaitToCharge(closestBody));
            yield break;
        }

        Debug.Log("There's no one left for the zombies to kill");
        StartCoroutine(WaitToCharge(Vector3.zero));



    }

    IEnumerator WaitToCharge(Vector3 target)
    {
        Debug.Log("Waiting to attack");
        zombie.anim.SetTrigger("Run");

        yield return new WaitForSeconds(6);

        HeadToClosestBody(target);
    }

}

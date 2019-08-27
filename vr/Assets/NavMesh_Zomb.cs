using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class NavMesh_Zomb : MonoBehaviour
{
    public NavMeshAgent agent;
    public BodyInteract[] bodyGuards;
    int layerMask = 1 << 9;
    public Transform raycastPoint;
    public Animator anim;
    public RagDollController raggy; 


    // Start is called before the first frame update
    void Start()
    {
        raggy = GetComponent<RagDollController>();
        bodyGuards = FindObjectsOfType<BodyInteract>();
        agent = GetComponent<NavMeshAgent>();

        if (!anim)
        { 
        anim = GetComponentInChildren<Animator>();
        }

     
        HeadToClosestBody();
    }

   

    private void FixedUpdate()
    {
        DrawRay();


        if(raggy.TurnOn == false)
        {
            agent.enabled = false; 
        }
    }


    void Attack()
    {
        agent.isStopped = true;
        anim.SetTrigger("Attack");
        StartCoroutine(WaitASec());



    }

    void DrawRay()
    {
        Vector3 fwd = raycastPoint.position + raycastPoint.forward * 1f;

        Debug.DrawLine(raycastPoint.position, fwd);

        if(Physics.Linecast(raycastPoint.position, fwd, layerMask))
        {

            Attack();

        }
    }

    void HeadToClosestBody()
    {
        Vector3 closestBody = GetClosestBody().position;
        agent.SetDestination(closestBody);
        anim.SetTrigger("Run");

        transform.LookAt(closestBody);

    }


    Transform GetClosestBody()
    {
        List<Transform> bodyPositions = new List<Transform>();


        for (int i = 0; i < bodyGuards.Length; i++)
        {
            if(!bodyGuards[i].hit)
            {
                bodyPositions.Add(bodyGuards[i].transform);

            }

        }

        return bodyPositions[GetClosestDistances(bodyPositions)];

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


    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(2f);
        anim.ResetTrigger("Attack");
        HeadToClosestBody();
        yield break;
    }

}

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
        HeadToClosestBody();

    }

   

    private void FixedUpdate()
    {
        if (isBodyInFront())
            Attack();

        if(kill)
        {
            zombie.Interact(Vector3.zero);
            kill = false;
        }
    }


    void Attack()
    {
        zombie.agent.isStopped = true;
        zombie.anim.SetTrigger("Attack");
        StartCoroutine(WaitASec());



    }

    bool isBodyInFront()
    {
        Vector3 fwd = raycastPoint.position + raycastPoint.forward * 1f;

        Debug.DrawLine(raycastPoint.position, fwd);

        return Physics.Linecast(raycastPoint.position, fwd, layerMask);
        
    }

    void HeadToClosestBody()
    {
        Vector3 closestBody = GetClosestBody().position;
        zombie.agent.SetDestination(closestBody);
        zombie.anim.SetTrigger("Run");

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
        zombie.anim.ResetTrigger("Attack");

        if(isBodyInFront())
        {
            Attack();
            yield break; 
        }
        
        HeadToClosestBody();
    }

}

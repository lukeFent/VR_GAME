using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMesh_Zomb : MonoBehaviour
{
    public BodyInteract[] bodyGuards;
    int layerMask = 1 << 10;
    public Transform raycastPoint;

    public Transform target; 

	public float waitTime = 6f;

    public float huntWaitTime = 2f;

    public bool kill = false;
    public int life;
    public float speed;
    public ZombInteract zombie;
    public CheapFirstPersonShooter playerScript;



    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheapFirstPersonShooter>();
        zombie = GetComponent<ZombInteract>();
        bodyGuards = FindObjectsOfType<BodyInteract>();
        //HeadToClosestBody(GetClosestBody().position);
        StartCoroutine(FindThePrey());

    }


    private void Update()
    {
        if (kill)
        {
            playerScript.deadEnemies += 1;
            kill = false;
        }
    }

    private void FixedUpdate()
    {
        zombie.anim.speed = speed;
        zombie.agent.speed = speed;

    }


    IEnumerator FindThePrey()
    {
        target = GetClosestBody();
        if (kill)     
            yield break;
        
        if (isBodyInFront())
        {
            zombie.agent.enabled = false; 
            transform.LookAt(target);
            Attack();
            yield break;
        }
        else
            try
             { 
            zombie.agent.SetDestination(target.position);
            }

            catch
            {
                Debug.Log("You lose");
            }

            //zombie.agent.SetDestination(target.position);

            //HeadToClosestBody(target.position);

            yield return new WaitForSeconds(huntWaitTime);

        StartCoroutine(FindThePrey());

    }

    //for the animator--ZombIdle script

    public void FP()
    {
        zombie.agent.enabled = true; 
        StartCoroutine(FindThePrey());

    }

    void Attack()
    {
        Debug.Log("Attack");
        transform.LookAt(target);
        zombie.anim.applyRootMotion = true;
        zombie.anim.SetTrigger("Attack");
        //StartCoroutine(LookAround());



    }

  

    #region Looking for civilans

    bool isBodyInFront()
    {
        Vector3 fwd = raycastPoint.position + raycastPoint.forward * 1f;

        Debug.DrawLine(raycastPoint.position, fwd);

        return Physics.Linecast(raycastPoint.position, fwd, layerMask);

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

        Transform t = bodyPositions[GetClosestDistances(bodyPositions)];

        return t;

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

    #endregion

    IEnumerator LookAround()
    {
       
        //yield return new WaitForSeconds(0.25f);
        //zombie.anim.applyRootMotion = false;


        Debug.Log("Looking around");

        if (GetClosestBody() != null)
        {
            //Vector3 closestBody = GetClosestBody().position;
            zombie.anim.ResetTrigger("Attack");
            //float lookAngle = Vector3.Angle(target.position, transform.forward);
            //zombie.anim.SetFloat("Direction", -lookAngle);


            StartCoroutine(WaitToCharge(target.position));
            yield break;
        }

        Debug.Log("There's no one left for the zombies to kill");
        StartCoroutine(WaitToCharge(Vector3.zero));



    }

    IEnumerator WaitToCharge(Vector3 t)
    {
        Debug.Log("Waiting to attack");
        zombie.anim.speed = 1 *  waitTime;
        float animationTime = 0;

        //while(animationTime < 2)
        //{
        //    Debug.Log(animationTime);
        //    animationTime += Time.deltaTime;
        //    yield return null; 
        //}
        //zombie.anim.applyRootMotion = false;

        //zombie.anim.applyRootMotion = false; 
        yield return new WaitForSeconds(waitTime);
        zombie.anim.SetTrigger("Run");

        StartCoroutine(FindThePrey());

        //HeadToClosestBody(target);
    }
    void Death()
    {
     playerScript.deadEnemies += 1;
    }
}

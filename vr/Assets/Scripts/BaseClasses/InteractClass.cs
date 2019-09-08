using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InteractClass : MonoBehaviour
{
    public RagDollController ragdoll;
    public NavMeshAgent agent;
    public Animator anim; 


    public virtual void Start()
    {
      
     anim = GetComponentInChildren<Animator>();
     ragdoll = GetComponent<RagDollController>();
     agent = GetComponent<NavMeshAgent>();
        
    }

   public virtual void Interact(Vector3 direction)
    {
        Debug.Log("Hey");
        agent.enabled = false;
        anim.enabled = false; 
        ragdoll.KnockAbout(direction);

    }
}

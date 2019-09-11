using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombInteract : InteractClass
{


    //if we want the zombies to interact in a special way, we'll put it in here

    public override void Interact(Vector3 direction)
    {
        base.Interact(direction);
       
    }

    public void UpdatePosition()
    {
        transform.position = anim.rootPosition; 
    }
}

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

   



    


}

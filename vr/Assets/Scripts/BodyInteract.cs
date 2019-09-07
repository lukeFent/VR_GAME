using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInteract : InteractClass
{


    RagDollController rag;

    [SerializeField]
    Transform raycastPoint;

    bool rotate = false; 
    bool intercepted = false; 
    public Animator anim;
    public float speed;


    public bool hit = false;
    


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rag = GetComponentInChildren<RagDollController>();
    }


 

    public override void Interact(Vector3 direction)
    {
        intercepted = true; 
        rag.KnockAbout();
        hit = true; 
        //rag.KnockBack(direction);
    }

   



    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour
{

    public Animator ani; 
    public Rigidbody[] ragDoll = new Rigidbody[13];
    public BoxCollider box; 
    

    public bool TurnOn = true;

    public void InteractWithMe()
    {
        TurnOnRagDoll(false);
    }

    public void TurnOnRagDoll(bool value)
    {
        for (int i = 0; i < ragDoll.Length; i++)
        {
            ragDoll[i].isKinematic = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        TurnOnRagDoll(true);
    }

    public void KnockAbout()
    {
        box.enabled = false;
        ani.enabled = false;
        TurnOnRagDoll(false);
    }


    // Update is called once per frame
    void Update()
    {
        if(TurnOn == false)
        {
            ani.enabled = false; 
            TurnOnRagDoll(false);
        }

    }
}


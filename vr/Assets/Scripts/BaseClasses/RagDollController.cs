using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour
{
    public Rigidbody[] ragDoll = new Rigidbody[11];
    public BoxCollider box;



    void Start()
    {

        SetRagDollKinesmatic(true);
    }



    public void SetRagDollKinesmatic(bool value)
    {
        for (int i = 0; i < ragDoll.Length; i++)
        {
            ragDoll[i].isKinematic = value;
        }

     
    }

    public void KnockAbout(Vector3 direction)
    {
        box.enabled = false;
        SetRagDollKinesmatic(false);
        //KnockBack(direcion); 
    }

    //alt version of knock back that adds explosive force. Doesn't really work that well unfortunately. 
    public void KnockBack(Vector3 direction)
    {
        for(int i = 0; i < ragDoll.Length; i++)
        {
            ragDoll[i].AddExplosionForce(10, direction, 5);
        }
    }

  


 

}


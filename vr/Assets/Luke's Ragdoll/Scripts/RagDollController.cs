using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour
{

    public Animator ani; 
    public Rigidbody[] ragDoll = new Rigidbody[11];
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


    public void PickYourselfUp()
    {
        StartCoroutine(GetUp());
    }

    IEnumerator GetUp()
    {
          yield return new WaitForSeconds(0.05f);

        box.enabled = true;
        TurnOnRagDoll(true);
       // ani.enabled = true;

    }


    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();

        TurnOnRagDoll(true);
    }

    public void KnockBack(Vector3 direction)
    {
        for(int i = 0; i < ragDoll.Length; i++)
        {
            ragDoll[i].AddExplosionForce(10, direction, 5);
        }
    }

    public void KnockAbout()
    {
        box.enabled = false;
        ani.enabled = false;
        TurnOnRagDoll(false);

        //StartCoroutine(TurnRagDollOffAfterASec());
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

    IEnumerator TurnRagDollOffAfterASec()
    {
        yield return new WaitForSeconds(2f);
        TurnOnRagDoll(true);


    }

}


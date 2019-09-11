using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombAnimator : MonoBehaviour
{

    public bool canMove = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    //public void MoveTransform()
    //{
    //    transform.parent.position = GetComponent<Animator>().rootPosition;

    //}

    private void Update()
    {
        if(anim.applyRootMotion == true)
        {
            Debug.Log("Root motion is true");
        }
    }

    private void OnAnimatorMove()
    {

        if(anim.applyRootMotion == true)
        { 
        transform.parent.position = GetComponent<Animator>().rootPosition;
        transform.parent.rotation = GetComponent<Animator>().rootRotation;
        }


    }
}

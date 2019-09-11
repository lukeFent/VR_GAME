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

    private void OnAnimatorMove()
    {

        //if (canMove)
        //{
        //    transform.parent.position = GetComponent<Animator>().rootPosition;
        //    Debug.Log("Move");

        //    canMove = false;
        //}

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBehaviour : MonoBehaviour
{

    public Animator anim;
    public float speed; 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetTrigger("Walk");
    }

    void Patrol()
    {
        transform.position += transform.forward * speed * Time.deltaTime;  
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }



}

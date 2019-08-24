using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float thrust = 10; 
    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Fired()
    {
        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * thrust, ForceMode.Impulse);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<InteractClass>())
        {
            other.transform.GetComponent<InteractClass>().Interact();
        }
    }
}

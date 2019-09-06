using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float thrust; 
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
            body.mass = 1;
            Vector3 heading = transform.position - other.transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance; 


            other.transform.GetComponent<InteractClass>().Interact(direction);
        }
    }
}

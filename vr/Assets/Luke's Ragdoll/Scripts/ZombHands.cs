using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombHands : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<InteractClass>())
        {
            Vector3 heading = transform.position - other.transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;

            
            other.transform.GetComponent<InteractClass>().Interact(direction);
        }
    }


}

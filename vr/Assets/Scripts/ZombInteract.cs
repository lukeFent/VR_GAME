using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombInteract : MonoBehaviour
{
    public Transform throwPoint;
	public GameObject bulletPrefab;

    void Start()
	{
        //StartCoroutine(Fire());
	}

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(2f);
        Shoot();
    }

    public void Shoot()
	{
		GameObject bullet = Instantiate(bulletPrefab, throwPoint.position, transform.rotation);
		bullet.GetComponent<Bullet>().Fired();
	}


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

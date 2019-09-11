using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float thrust; 
    private Rigidbody body;
    public float distance;

    //public GameObject bloodEffect;
    public ParticleSystem blood;

    private void Start()
    {
        blood = GetComponentInChildren<ParticleSystem>();
        blood.Stop();
        //bloodEffect.SetActive(false);

        body = GetComponent<Rigidbody>();
        StartCoroutine(KillBullet(distance));
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
            blood.Play(); 
            //bloodEffect.SetActive(true);
            body.mass = 1;
            Vector3 heading = transform.position - other.transform.position;
            Vector3 direction = heading / heading.magnitude;

            other.transform.GetComponent<InteractClass>().Interact(direction);

            if (other.GetComponent<EnemyIndividualBehavior>())
            {
                other.GetComponent<EnemyIndividualBehavior>().kill = true;
            }
            if (other.GetComponent<NavMesh_Zomb>())
            {
                other.GetComponent<NavMesh_Zomb>().kill = true;
            }
        }
    }
    IEnumerator KillBullet(float x)
    {
        yield return new WaitForSeconds(x);
        Destroy(this.gameObject);
    }
}

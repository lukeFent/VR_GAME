using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndividualBehavior : MonoBehaviour
{
    public string enemyName;
    public int life = 2;
    public float speed;
    public CheapFirstPersonShooter playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheapFirstPersonShooter>();
    }

    private void Update()
    {
        Death();
    }
    public EnemyIndividualBehavior(string newName, int newPower, float newSpeed)
    {
        enemyName = newName;
        life = newPower;
        speed = newSpeed;
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Bullet")
        {
            life -= 1;
        }
    }

    void Death()
    {
        if(life <= 0)
        {
            playerScript.deadEnemies += 1;
        }
    }
}

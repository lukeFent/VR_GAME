using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndividualBehavior : MonoBehaviour
{
    public string enemyName;
    public int life = 2;
    public float speed;
    public bool kill = false;
    public CheapFirstPersonShooter playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheapFirstPersonShooter>();
    }

    private void Update()
    {
       
        if (kill)
        {
            playerScript.deadPeople += 1;
            kill = false;
        }
    }
    public EnemyIndividualBehavior(string newName, int newPower, float newSpeed)
    {
        enemyName = newName;
        life = newPower;
        speed = newSpeed;
    }


 
}

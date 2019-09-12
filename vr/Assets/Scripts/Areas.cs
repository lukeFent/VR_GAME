using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour
{

    #region Luke's waypoint system

    public delegate void AreaCleared();
    public  event AreaCleared OnAreaCleared; 

    public CivilianManager civilianManager;

    #endregion

    public GameObject[] zombies;
    public List<GameObject> z_list = new List<GameObject>();


    public float respawnTimer = 1;
    bool onlyOne = false;

    //For managing respawn points in each area
    public GameObject[] spawnPoints;
    public List<GameObject> z_Spawn = new List<GameObject>();

    //for managing player waypoints per area
    GameObject[] wp_Waypoints;
    public List<GameObject> cowerPoints = new List<GameObject>();


    public CheapFirstPersonShooter player;
    public bool cleared = false;

    private void Start()
    {
        civilianManager = CivilianManager.singleton;
        FindWaypoints();
        FindSpawnPoints();
        FindZombies();
    }

    void Update()
    {
        cleared = player.cleared;
        if (cleared)
        {
            civilianManager.FollowPlayer();
            player.cleared = cleared;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= 8)
        {
            SetZombieSpeed(0, 1.5f, this.gameObject.GetComponent<Areas>().z_list);
        }
    }

    void ChangeCowerPoints()
    {
        if (Vector3.Distance(player.transform.position, transform.position) >= 1)
        {
            FindWaypoints();
        }
        civilianManager.RunToAreaWaypoints(cowerPoints);
    }


    void FindWaypoints()
    {
        wp_Waypoints = GameObject.FindGameObjectsWithTag("WP");
        foreach (var item in wp_Waypoints)
        {
            if (Vector3.Distance(item.transform.position, this.gameObject.transform.position) <= 10)
            {
                cowerPoints.Add(item);
            }
        }
    }

    void FindSpawnPoints()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SP");
        foreach (var item in spawnPoints)
        {
            if (Vector3.Distance(item.transform.position, this.gameObject.transform.position) <= 20)
            {
                z_Spawn.Add(item);
            }
        }
        Debug.Log("There are  " + z_Spawn.Count + "  Zombie Spawn points available at " + this.gameObject.ToString());
    }

    void SetZombieSpeed(float x, float y, List<GameObject> z)
    {
        foreach (var item in z)
        {
            if (Vector3.Distance(item.transform.position, this.gameObject.transform.position) <= 30)
            {
                item.GetComponent<NavMesh_Zomb>().speed = Random.Range(x, y);
            }
        }
    }

    void FindZombies()
    {
        zombies = GameObject.FindGameObjectsWithTag("Enemies");
        foreach (var item in zombies)
        {
            if (Vector3.Distance(item.transform.position, this.gameObject.transform.position) <= 30)
            {
                z_list.Add(item);
                int i = Random.Range(0, z_Spawn.Count);
                item.transform.position = z_Spawn[i].transform.position;
                item.GetComponent<NavMesh_Zomb>().speed = 0;
            }
        }
    }
}

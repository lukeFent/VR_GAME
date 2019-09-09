using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Areas : MonoBehaviour
{

    public GameObject[] zombies;
    public GameObject zombie;
    public Transform[] spawnPoints;
    public GameObject[] people;
    GameObject[] areas;
    public float respawnTimer = 1;
    bool onlyOne = false;
    public AudioSource bg_Music;
    public float bgM_Volume;

    private void Start()
    {
        
    }
    void Update()
    {
        bg_Music.volume = bgM_Volume;
        //foreach (var z in zombies)
        //{
        //    if (z.GetComponent<NavMesh_Zomb>().kill)
        //    {
        //       Respawn(z);
        //    }
        //}
    }

    void Respawn(GameObject z)
    {
        int i = 0;
        i = Random.Range(0, spawnPoints.Length);
        StartCoroutine(RTimer(respawnTimer, z, i));
    }

    IEnumerator RTimer(float timer, GameObject z, int i)
    {

        yield return new WaitForSeconds(timer);
        GameObject argh = Instantiate(zombie, spawnPoints[i].position, transform.rotation);

    }
}

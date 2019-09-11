using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianManager : MonoBehaviour
{

   
    public BodyInteract[] civilians = new BodyInteract[4];
    public GameObject[] areas;

    [Header("Around the player waypoints")]
    public WayPoint[] aroundPlayerWP = new WayPoint[4];

    [Header("Area waypoints")]
    public WayPoint[] areaWP = new WayPoint[4];

    [Header("Distance from player")]
    public float distanceFromPlayer = 2;

    public bool r_ToPlayer = true;
    public Queue<List<GameObject>> CP_Overide = new Queue<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
       RunToPlayer();
    }

    private void Update()
    {

    }
    public void RunToPlayer()
    {
        for (int i = 0; i < aroundPlayerWP.Length; i++)
        {
            if (!civilians[i].HasArrived(aroundPlayerWP[i].transform.position))
            {
                civilians[i].HeadToSafety(aroundPlayerWP[i].transform.position, distanceFromPlayer);
            }
            if(i >= aroundPlayerWP.Length)
            {
                foreach (var item in areas)
                {
                    gameObject.GetComponent<Areas>().cleared = false;
                }
            }
        }
    }


    public void RunToAreaWaypoints(List<GameObject> k)
    {
        for (int i = 0; i < k.Count; i++)
        {
            if (!civilians[i].HasArrived(k[i].transform.position))
            {
                civilians[i].HeadToSafety(k[i].transform.position, distanceFromPlayer);
            }
            if (i >= k.Count)
            {
                foreach (var item in areas)
                {
                    gameObject.GetComponent<Areas>().cleared = false;
                }
            }
        }
      //  Debug.Log("Take Cover!!!");
    }

    public void WalkWithPlayer()
    {
      
    }

    public static CivilianManager singleton;

    private void Awake()
    {
        singleton = this;
    }

    void MakeAQueue(List<GameObject> X)
    {
        CP_Overide.Enqueue(X);
    }
}

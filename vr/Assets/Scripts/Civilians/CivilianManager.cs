using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianManager : MonoBehaviour
{

   
    public BodyInteract[] civilains = new BodyInteract[4];

    [Header("Around the player waypoints")]
    public WayPoint[] aroundPlayerWP = new WayPoint[4];

    [Header("Area 1 waypoints")]
    public WayPoint[] areaWP = new WayPoint[4];

    [Header("Distance from player")]
    public float distanceFromPlayer = 2; 

    // Start is called before the first frame update
    void Start()
    {
        RunToPlayer();
    }

    public void RunToPlayer()
    {
        for (int i = 0; i < aroundPlayerWP.Length; i++)
        {
            civilains[i].HeadToSafety(aroundPlayerWP[i].transform.position, distanceFromPlayer);
        }
    }


    public void RunToAreaWaypoints(WayPoint[] areaWP)
    {
        for (int i = 0; i < areaWP.Length; i++)
        {
            civilains[i].HeadToSafety(areaWP[i].transform.position, distanceFromPlayer);
        }
    }

    public void WalkWithPlayer()
    {
        //tbd
    }

    public static CivilianManager singleton;

    private void Awake()
    {
        singleton = this;
    }
}

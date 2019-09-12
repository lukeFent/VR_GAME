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

    public Queue<List<GameObject>> CP_Overide = new Queue<List<GameObject>>();

    bool canRun = false; 

    // Start is called before the first frame update
    void Start()
    {
       RunToPlayer();
    }

    private void OnEnable()
    {
        //Areas.AreaCleared += Gagh;
    }

    private void Update()
    {

    }

    public void CivilainsRun(bool value)
    {
 
        if(value != canRun)
        {
            RunToPlayer();
            canRun = value; 
        }
    }


    public void FollowPlayer()
    {
        foreach (BodyInteract civ in civilians)
        {
            if (civ.agent.isActiveAndEnabled)
            {
                civ.anim.SetTrigger("Run");
                civ.agent.isStopped = false;

            }
        }

        StartCoroutine(FP());
    }

    IEnumerator FP()
    {
        if(CheapFirstPersonShooter.singleton.pickedUpShotgun == false)
        {
            for (int i = 0; i < aroundPlayerWP.Length; i++)
            {
                if (civilians[i].agent.isActiveAndEnabled)
                {
                civilians[i].agent.SetDestination(CheapFirstPersonShooter.singleton.transform.position);
                Debug.Log(CheapFirstPersonShooter.singleton.transform.position);
                }
                yield return null;
            }
        

        }

        else
        {
            RunToPlayer();
        }
    }


    public void RunToPlayer()
    {
        for (int i = 0; i < aroundPlayerWP.Length; i++)
        {
           
             civilians[i].HeadToSafety(aroundPlayerWP[i].transform.position, distanceFromPlayer);
            
            if(i >= aroundPlayerWP.Length)
            {
                foreach (var item in areas)
                {
                    canRun = false; 
                    gameObject.GetComponent<Areas>().cleared = false;
                }
            }
        }
    }


    public void RunToAreaWaypoints(List<GameObject> k)
    {
        for (int i = 0; i < k.Count; i++)
        {
         
            civilians[i].HeadToSafety(k[i].transform.position, distanceFromPlayer);
          
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

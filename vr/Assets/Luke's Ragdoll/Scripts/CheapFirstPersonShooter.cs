using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapFirstPersonShooter : MonoBehaviour
{

    public Transform bulletSpawn;
    public GameObject bulletPrefab; 

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothness = 2.0f;
    bool toggle = true; 
    GameObject character;

    public LineRenderer laz; 

    char key;


    public float zPos; 

    // Start is called before the first frame update
    void Start()
    {
        character = this.transform.parent.gameObject;
        //bulletSpawn.position = new Vector3(1 / (Screen.width / 2), 1 / (Screen.height / 2), transform.position.z);
    }

    void Look()
    {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothness, sensitivity * smoothness));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothness);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothness);
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);


    }

    void tog()
    {
        if (toggle)
            toggle = false;

        else
            toggle = true; 
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
        bullet.GetComponent<Bullet>().Fired();

    }


    // Update is called once per frame
    void Update()
    {

        if (toggle)
        Look();

        if (Input.GetKeyDown(KeyCode.Space))
            FireLaser();

        if (Input.GetKeyDown(KeyCode.Escape))
            tog();

        RaycastHit blip;

        if (Physics.Raycast(bulletSpawn.position, bulletSpawn.forward, out blip, Mathf.Infinity))
        {
            Debug.DrawLine(bulletSpawn.position, blip.point, Color.magenta);
            Debug.Log(blip.point);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                laz.SetPosition(0, Vector3.zero);
                laz.SetPosition(1, GetComponentInParent<Transform>().position);

                StartCoroutine(LaserBlast(laz, blip.point));
            }
        }
    }

    void FireLaser()
    {
        RaycastHit hit;
        if(Physics.Raycast(bulletSpawn.position, bulletSpawn.forward, out hit, Mathf.Infinity))
        {
            Vector3 heading = hit.point - transform.position;
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;


            laz.SetPosition(0, Vector3.zero);
            laz.SetPosition(1, GetComponentInParent<Transform>().position);

            StartCoroutine(LaserBlast(laz, hit.point));
        }
    }


    IEnumerator LaserBlast(LineRenderer r, Vector3 e)
    {
        while (Vector3.Distance(r.GetPosition(1), e) > 0.1)
        {
            r.SetPosition(1, Vector3.Lerp(r.GetPosition(1), e, 4f * Time.deltaTime));
            r.SetPosition(0, Vector3.Lerp(r.GetPosition(0), e, 2f * Time.deltaTime));

            yield return null;
        }

       
        Debug.Log("Finished couroutine");
    }

    IEnumerator LazBlast(float distance, Vector3 direction)
    {
        distance = 20; 
        while(distance > 1)
        {
            laz.SetPosition(0, laz.GetPosition(0) + direction);
            distance += -1; 
            yield return null; 

        }
    }

   
}

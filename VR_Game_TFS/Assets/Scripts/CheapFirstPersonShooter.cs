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

    // Start is called before the first frame update
    void Start()
    {
        character = this.transform.parent.gameObject;
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
        if(toggle)
        Look();

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        if (Input.GetKeyDown(KeyCode.Escape))
            tog();
    }
}

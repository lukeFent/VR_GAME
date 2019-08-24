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

    int jump = 0;
 
    public GameObject character;
    GameObject cam;
    Transform bodyHeight;
    Vector3 currentZPos;
    Rigidbody body;
    public float jumpHeight = 5;
    public float m_FB;
    public float m_LR;
    public float p_speed = 6;
    public float p_MS = 20;
    bool IsGrounded;
    [Range(0, .5f)]
    public float DeadZone = .028f;
    float xRotation;
    float yRotation;
    public float currentXRotation;
    public float currentYRotation;
    public float xRotationV;
    public float yRotationV;
    float r_X;
    float r_Y;

    //new stuff for moving around our level and level progression
    public GameObject[] enemies;
    public GameObject[] positions;

    //Camera Rotate on movement
    [Range(1, 5)]
    public float cameraRotateSpeed;
    //Area One
    int areaOneClear = 3;
    public int deadEnemies;

    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject;
        body = character.GetComponent<Rigidbody>();
        bodyHeight = character.transform;
        enemies = GameObject.FindGameObjectsWithTag("Enemies");
      //  positions = GameObject.FindGameObjectsWithTag("Positions");
    }

    void Look()
    {
        xRotation += r_Y * sensitivity;
        yRotation += r_X * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        yRotation = Mathf.Clamp(yRotation, -75, 80);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, smoothness);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, smoothness);
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);  
    }


    // Update is called once per frame
    void Update()
    {
        m_LR = Input.GetAxis("Horizontal");
        m_FB = Input.GetAxis("Vertical");
        r_X = Input.GetAxis("Mouse X");
        r_Y = Input.GetAxis("Mouse Y");
        if (r_X > DeadZone || r_X < -DeadZone || r_Y > DeadZone || r_Y < -DeadZone)
        {
            Look();
        }
        if(m_LR > DeadZone || m_LR < -DeadZone || m_FB > DeadZone || m_FB < -DeadZone)
        {
       // Move();
        }
        MovePosition();
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetButtonDown("Jump") && jump <= 1)
        {
            Jump();
        }
    }

    private void MovePosition()
    {
        if (deadEnemies >= areaOneClear)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, positions[1].transform.position, p_speed * Time.deltaTime);
            Vector3 newDir1 = Vector3.RotateTowards(character.transform.forward, positions[1].transform.position - character.transform.position, cameraRotateSpeed * Time.deltaTime, 0.0f);
            character.transform.rotation = Quaternion.LookRotation(newDir1);
        }
        else if (deadEnemies < areaOneClear)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, positions[0].transform.position, p_speed * Time.deltaTime);
            Vector3 newDir0 = Vector3.RotateTowards(character.transform.forward, positions[0].transform.position - character.transform.position, cameraRotateSpeed * Time.deltaTime, 0.0f);
            character.transform.rotation = Quaternion.LookRotation(newDir0);
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
        bullet.GetComponent<Bullet>().Fired();
    }
    void Jump()
    {
        jump += 1;
        body.AddForce(Vector3.up * jumpHeight * Time.deltaTime, ForceMode.Impulse);
        IsGrounded = false;
    }

    void Move()
    {
        character.transform.Translate(Vector3.right * Time.deltaTime * m_LR * (p_speed));
        character.transform.Translate(Vector3.forward * Time.deltaTime * m_FB * p_speed);
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Ground")
        {
            IsGrounded = true;
            jump = 0;
        }
    }
}

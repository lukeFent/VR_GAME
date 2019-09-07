using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapFirstPersonShooter : MonoBehaviour
{
     
    public GameObject character; //The cameras body Model and support rigs
    GameObject cam; //first person view, for both Controller and VR support
    Rigidbody body;
    public bool VR_Mode;
    #region First Person Controller
    //Movement for Controller Support
    public float jumpHeight = 5;
    public float m_FB;
    public float m_LR;
    public float p_speed = 6;
    public float p_MS = 20;

    //Not Currently in use
    bool IsGrounded;
    int jump = 0;

    //Sensitivity and Smoothing for Controller Support
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothness = 2.0f;

    //Rotation Management
    [Range(0, .5f)]
    float DeadZone = .028f;
    float xRotation;
    float yRotation;
    float currentXRotation;
    float currentYRotation;
    float xRotationV;
    float yRotationV;
    float r_X;
    float r_Y;

    //First Person Headbob 
    [Range(0, 4)]
    public float h_drop;
    [Range(0, 4)]
    public float h_rise;

    //Camera Rotate on movePosition()
    [Range(1, 5)]
    public float cameraRotateSpeed;
    #endregion

    #region Area Progression
    //stuff for moving around our level and level progression
    public GameObject[] enemies;
    public GameObject[] positions;

    #region AreaOne
    //Zombie Conditions
    int areaOneClear = 3;
    public int deadEnemies;
    //People Conditions
    int areaOneFail = 3;
    public int deadPeople;
    #endregion

    #region AreaTwo
    //Zombie Conditions
    int areaTwoClear = 8;
    //People Conditions
    int areaTwoFail = 4;
    #endregion

    #endregion

    #region Weapons
    //Shooting and Bullets
    public Transform bulletSpawn;
    public GameObject bulletPrefab;

    public GameObject[] weaponsInGame;
    public Queue<GameObject> currentWeapons = new Queue<GameObject>();
    Queue<GameObject> weaponHolster = new Queue<GameObject>();

    public bool pickedUpShotgun = false;
    public bool pickedUpAK = false;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject;
        body = character.GetComponent<Rigidbody>();
        enemies = GameObject.FindGameObjectsWithTag("Enemies");
        currentWeapons.Enqueue(weaponsInGame[0]);
        currentWeapons.Peek().gameObject.SetActive(true);
    }

    void Look()
    {
        xRotation += r_Y * sensitivity;
        yRotation += r_X * sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, smoothness);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, smoothness);
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        character.transform.rotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
    }


    // Update is called once per frame
    void Update()
    {
        m_LR = Input.GetAxis("Horizontal");
        m_FB = Input.GetAxis("Vertical");
        r_X = Input.GetAxis("Mouse X");
        r_Y = Input.GetAxis("Mouse Y");


        SwitchWeapons();
        if (r_X > DeadZone || r_X < -DeadZone || r_Y > DeadZone || r_Y < -DeadZone)
        {
            Look();
        }

        if (!VR_Mode)
        {
            if (m_LR > DeadZone || m_LR < -DeadZone || m_FB > DeadZone || m_FB < -DeadZone)
            {
                HeadBob();
                Move();
            }
            else if (m_LR < DeadZone || m_LR > -DeadZone || m_FB < DeadZone || m_FB > -DeadZone)
            {

            }
        }

        #region VR Movement and Weapon funtions
        if (VR_Mode)
        {

            MovePosition();
        }
        #endregion

        //for some reason the shotgun broke using this logic, will try to fix, it will equip but wont fire
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (currentWeapons.Peek().gameObject.tag == "Pistol")
            {
                currentWeapons.Peek().gameObject.GetComponent<Pistol>().Shooting();
            }
            else if (currentWeapons.Peek().gameObject.tag == "Shotgun")
            {
                currentWeapons.Peek().gameObject.GetComponent<Shotgun>().Shooting();
            }
            else if (currentWeapons.Peek().gameObject.tag == "AK47")
            {
                currentWeapons.Peek().gameObject.GetComponent<AK47>().Shooting();
            }
        }
        if (Input.GetButtonDown("Jump") && jump <= 1)
        {
            Jump();
        }
    }

    private void MovePosition()
    {
        if (deadEnemies >= areaOneClear && deadEnemies < areaTwoClear && Vector3.Distance(character.transform.position, positions[1].transform.position) >= 1f && !pickedUpShotgun)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, positions[1].transform.position, p_speed * Time.deltaTime);
            Vector3 newDir1 = Vector3.RotateTowards(cam.transform.forward, positions[1].transform.position - character.transform.position, cameraRotateSpeed * Time.deltaTime, 0.0f);
            cam.transform.rotation = Quaternion.LookRotation(newDir1);
            HeadBob();
        }
        else if (deadEnemies >= areaTwoClear && Vector3.Distance(character.transform.position, positions[2].transform.position) >= 1f && !pickedUpAK)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, positions[2].transform.position, p_speed * Time.deltaTime);
            Vector3 newDir1 = Vector3.RotateTowards(cam.transform.forward, positions[2].transform.position - character.transform.position, cameraRotateSpeed * Time.deltaTime, 0.0f);
            cam.transform.rotation = Quaternion.LookRotation(newDir1);
            HeadBob();
        }
        else if (deadEnemies < areaOneClear && Vector3.Distance(character.transform.position, positions[0].transform.position) >= 1f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, positions[0].transform.position, p_speed * Time.deltaTime);
            Vector3 newDir0 = Vector3.RotateTowards(cam.transform.forward, positions[0].transform.position - character.transform.position, cameraRotateSpeed * Time.deltaTime, 0.0f);
            cam.transform.rotation = Quaternion.LookRotation(newDir0);
            HeadBob();
        }
    }

    void SwitchWeapons()
    {
        if (VR_Mode)
        {
            if (!pickedUpShotgun)
            {
                if (Vector3.Distance(character.transform.position, positions[1].transform.position) <= 1)
                {
                    weaponHolster.Enqueue(currentWeapons.Peek().gameObject);
                    currentWeapons.Peek().gameObject.SetActive(false);
                    currentWeapons.Dequeue();
                    currentWeapons.Enqueue(weaponsInGame[1].gameObject);
                    currentWeapons.Peek().gameObject.SetActive(true);
                    pickedUpShotgun = true;
                }
            }
            if (!pickedUpAK)
            {
                if (Vector3.Distance(character.transform.position, positions[2].transform.position) <= 1)
                {
                    weaponHolster.Enqueue(currentWeapons.Peek().gameObject);
                    currentWeapons.Peek().gameObject.SetActive(false);
                    currentWeapons.Dequeue();
                    currentWeapons.Enqueue(weaponsInGame[2].gameObject);
                    currentWeapons.Peek().gameObject.SetActive(true);
                    pickedUpAK = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && pickedUpShotgun)
            {
                weaponHolster.Enqueue(currentWeapons.Peek().gameObject);
                currentWeapons.Peek().gameObject.SetActive(false);
                currentWeapons.Enqueue(weaponHolster.Peek().gameObject);
                currentWeapons.Dequeue();
                weaponHolster.Dequeue();
                currentWeapons.Peek().gameObject.SetActive(true);
            }
        }
        else if (!VR_Mode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weaponHolster.Enqueue(currentWeapons.Peek().gameObject);
                currentWeapons.Peek().gameObject.SetActive(false);
                currentWeapons.Enqueue(weaponHolster.Peek().gameObject);
                currentWeapons.Dequeue();
                weaponHolster.Dequeue();
                currentWeapons.Peek().gameObject.SetActive(true);
            }
        }
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

    void HeadBob()
    {
      cam.transform.position = new Vector3(cam.transform.position.x, Mathf.Lerp(h_drop, h_rise, Mathf.PingPong(Time.time, 0.3f)), cam.transform.position.z);
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
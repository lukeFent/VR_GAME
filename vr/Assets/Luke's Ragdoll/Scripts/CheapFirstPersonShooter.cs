﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheapFirstPersonShooter : MonoBehaviour
{
 
    public GameObject character; //The cameras body Model and support rigs
    GameObject cam; //first person view, for both Controller and VR support
    Rigidbody body;

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

    #endregion

    #region Weapons
    //Shooting and Bullets
    public Transform bulletSpawn;
    public GameObject bulletPrefab;

    public GameObject[] weaponsInGame;
    public List<GameObject> currentWeapons = new List<GameObject>();

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        cam = this.gameObject;
        body = character.GetComponent<Rigidbody>();
        enemies = GameObject.FindGameObjectsWithTag("Enemies");
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

        if (r_X > DeadZone || r_X < -DeadZone || r_Y > DeadZone || r_Y < -DeadZone)
        {
            Look();
        }
        if (m_LR > DeadZone || m_LR < -DeadZone || m_FB > DeadZone || m_FB < -DeadZone)
        {
            HeadBob();
            Move();
        }
        else if (m_LR < DeadZone || m_LR > -DeadZone || m_FB < DeadZone || m_FB > -DeadZone)
        {

        }
        MovePosition();
        if (Input.GetButtonDown("Fire1") || Input.GetKey(KeyCode.LeftShift))
        {
            //GameObject.FindGameObjectWithTag("weapon").GetComponent<Pistol>().Shooting();
            GameObject.FindGameObjectWithTag("weapon").GetComponent<Shotgun>().Shooting();
        }
        if (Input.GetButtonDown("Jump") && jump <= 1)
        {
            Jump();
        }
    }

    private void MovePosition()
    {
        if (deadEnemies >= areaOneClear && Vector3.Distance(character.transform.position, positions[1].transform.position) >= 1f)
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, positions[1].transform.position, p_speed * Time.deltaTime);
            Vector3 newDir1 = Vector3.RotateTowards(cam.transform.forward, positions[1].transform.position - character.transform.position, cameraRotateSpeed * Time.deltaTime, 0.0f);
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
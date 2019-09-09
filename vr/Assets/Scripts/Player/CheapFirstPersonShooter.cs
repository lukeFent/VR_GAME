using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheapFirstPersonShooter : MonoBehaviour
{
     
    public GameObject character; //The cameras body Model and support rigs
    GameObject cam; //first person view, for both Controller and VR support
    Rigidbody body;
    public bool VR_Mode;

    #region UI
    public GameObject reloadUI;
    public Text shotsRemaining;
    public Text totalAmmo;
    public Text zombiesKilled;
    public Text peopleKilled;
    public GameObject outOfAmmo;
    public GameObject AreaClear;

    bool clearedOne = true;
    bool clearedTwo = true;
    bool clearedThree = true;
    bool cleared = false;

    #endregion
    #region First Person Controller
    //Movement for Controller Support
    float jumpHeight = 5;
    float m_FB;
    float m_LR;
    float p_speed = 6;
    float p_MS = 20;

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
    float h_drop;
    [Range(0, 4)]
    float h_rise;

    //Camera Rotate on movePosition()
    [Range(1, 5)]
    public float cameraRotateSpeed;
    #endregion



    #region Area Progression

    public int deadEnemies;
    public int deadPeople;
    #region AreaOne
    //Zombie Conditions
    int areaOneClear = 3;
    //People Conditions
    int areaOneFail = 3;
    #endregion

    #region AreaTwo
    //Zombie Conditions
    int areaTwoClear = 8;
    //People Conditions
    int areaTwoFail = 4;
    #endregion

    #region AreaThree
    int areaThreeClear = 12;
    int areThreeFail = 6;
    #endregion

    //stuff for moving around our level and level progression
    public GameObject[] enemies;
    public GameObject[] positions;
    #endregion
    #region Weapons
    public GameObject[] weaponsInGame;
    public Queue<GameObject> currentWeapons = new Queue<GameObject>();
    Queue<GameObject> weaponHolster = new Queue<GameObject>();
    public float equipedWeaponReloadTime;

    bool isReloading;
    bool pickedUpShotgun = false;
    bool pickedUpAK = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        reloadUI.SetActive(false);
        outOfAmmo.SetActive(false);
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
        r_X = Input.GetAxis("Mouse X") + Input.GetAxis("CamX");
        r_Y = Input.GetAxis("Mouse Y") + Input.GetAxis("CamY");

        SetUI();
        SwitchWeapons();

        //this is strictly for testing
        if(deadEnemies < areaOneClear)
        {
            clearedOne = true;
            clearedTwo = true;
            clearedThree = true;
        }
        //end of test function

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
                Look();
            }
        }

        #region VR Movement and Weapon funtions
        if (VR_Mode)
        {
            MovePosition();
        }
        #endregion
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            if (currentWeapons.Peek().gameObject.tag == "Pistol")
            {
                currentWeapons.Peek().gameObject.GetComponent<Pistol>().Shooting();
                equipedWeaponReloadTime = currentWeapons.Peek().gameObject.GetComponent<Pistol>().reloadTime;
            }
            else if (currentWeapons.Peek().gameObject.tag == "Shotgun")
            {
                currentWeapons.Peek().gameObject.GetComponent<Shotgun>().Shooting();
                equipedWeaponReloadTime = currentWeapons.Peek().gameObject.GetComponent<Shotgun>().reloadTime;
            }
            else if (currentWeapons.Peek().gameObject.tag == "AK47")
            {
                currentWeapons.Peek().gameObject.GetComponent<AK47>().Shooting();
                equipedWeaponReloadTime = currentWeapons.Peek().gameObject.GetComponent<AK47>().reloadTime;
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
            if (Input.GetKeyDown(KeyCode.Alpha1) && pickedUpShotgun && !isReloading)
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

    void SetUI()
    {
        if (currentWeapons.Count >= 1)
        {
            if (currentWeapons.Peek().gameObject.tag == "Pistol")
            {
                totalAmmo.text = "Total Ammo: " + currentWeapons.Peek().gameObject.GetComponent<Pistol>().ammo.ToString();
                shotsRemaining.text = "Ammo: " + currentWeapons.Peek().gameObject.GetComponent<Pistol>().ammoInClip.ToString();
                isReloading = currentWeapons.Peek().gameObject.GetComponent<Pistol>().reloading;
                if (currentWeapons.Peek().gameObject.GetComponent<Pistol>().ammo == 0)
                {
                    outOfAmmo.SetActive(true);
                }
                if (currentWeapons.Peek().gameObject.GetComponent<Pistol>().ammo > 0)
                {
                    outOfAmmo.SetActive(false);
                }
            }
            else if (currentWeapons.Peek().gameObject.tag == "Shotgun")
            {
                totalAmmo.text = "Total Ammo: " + currentWeapons.Peek().gameObject.GetComponent<Shotgun>().ammo.ToString();
                shotsRemaining.text = "Ammo: " + currentWeapons.Peek().gameObject.GetComponent<Shotgun>().ammoInClip.ToString();
                isReloading = currentWeapons.Peek().gameObject.GetComponent<Shotgun>().reloading;
                if (currentWeapons.Peek().gameObject.GetComponent<Shotgun>().ammo == 0)
                {
                    outOfAmmo.SetActive(true);
                }
                if (currentWeapons.Peek().gameObject.GetComponent<Shotgun>().ammo > 0)
                {
                    outOfAmmo.SetActive(false);
                }
            }
            else if (currentWeapons.Peek().gameObject.tag == "AK47")
            {
                totalAmmo.text = "Total Ammo: " + currentWeapons.Peek().gameObject.GetComponent<AK47>().ammo.ToString();
                shotsRemaining.text = "Ammo: " + currentWeapons.Peek().gameObject.GetComponent<AK47>().ammoInClip.ToString();
                isReloading = currentWeapons.Peek().gameObject.GetComponent<AK47>().reloading;
                if (currentWeapons.Peek().gameObject.GetComponent<AK47>().ammo == 0)
                {
                    outOfAmmo.SetActive(true);
                }
                if (currentWeapons.Peek().gameObject.GetComponent<AK47>().ammo > 0)
                {
                    outOfAmmo.SetActive(false);
                }
            }
        }
        zombiesKilled.text = "Zombies Killed: " + deadEnemies;
        peopleKilled.text = "People Killed: " + deadPeople;

        if(clearedOne && deadEnemies >= areaOneClear && deadEnemies < areaTwoClear || clearedTwo && deadEnemies >= areaTwoClear && deadEnemies < areaThreeClear || clearedThree && deadEnemies >= areaThreeClear)
        {
            StartCoroutine(AreaCleared());
        }
    }


    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Ground")
        {
            IsGrounded = true;
            jump = 0;
        }
    }

    IEnumerator AreaCleared()
    {
        AreaClear.SetActive(true);
        yield return new WaitForSeconds(4);
        AreaClear.SetActive(false);
        if (deadEnemies >= areaOneClear && deadEnemies < areaTwoClear)
        {
            clearedOne = false;
        }
        if (deadEnemies >= areaTwoClear && deadEnemies < areaThreeClear)
        {
            clearedTwo = false;
        }
        if (deadEnemies >= areaThreeClear)
        {
            clearedThree = false;
        }
    }
}
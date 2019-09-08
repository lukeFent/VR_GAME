using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunClass : MonoBehaviour
{
    public string type;
    public int ammo;
    public int ammoInClip;
    public int maxClipSize;
    public float reloadTime;
    public float shootingSpeed;
    public float bulletSpeed;
    public float bulDist;
    public bool canShoot = true;
    public bool reloading = false;
    public int clipsUsed;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public CheapFirstPersonShooter playerScript;
    public Animation anim;
    public ParticleSystem particle;
  

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CheapFirstPersonShooter>();
    }

    public void Stats(string newName, int newMaxClip, int newAmmoInClip, int newAmmo, float newReloadTime, float newShootingSpeed, float newBulletSpeed, float bulletLife)
    {
        type = newName;
        ammo = newAmmo;
        maxClipSize = newMaxClip;
        ammoInClip = newAmmoInClip;
        reloadTime = newReloadTime;
        shootingSpeed = newShootingSpeed;
        bulletSpeed = newBulletSpeed;
        bulDist = bulletLife;
    }

    private void Update()
    {
        if (ammoInClip <= 0)
        {
            reloading = true;
            StartCoroutine(Reload(reloadTime));
        }
    }
    public void Shooting()
    {
        if(ammoInClip >= 1 && canShoot && !reloading)
        {
            StartCoroutine(ShootingSpeed(shootingSpeed));
        }    
    }

    public void Shoot()
    {   
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
        bullet.GetComponent<Bullet>().thrust = bulletSpeed;
        bullet.GetComponent<Bullet>().Fired();
        ammo -= 1;
    }

    IEnumerator ShootingSpeed(float x)
    {
        Shoot();
        anim.Play();
        if (particle != null)
        {
            StartCoroutine(MuzzleFlare(shootingSpeed));
        }
        canShoot = false;
        ammoInClip -= 1;
        yield return new WaitForSeconds(x);
        canShoot = true;
    }

    IEnumerator Reload(float x)
    {
        playerScript.reloadUI.SetActive(true);
        ammoInClip = maxClipSize;
        yield return new WaitForSeconds(x);
        reloading = false;
        playerScript.reloadUI.SetActive(false);
    }

    IEnumerator MuzzleFlare(float x)
    {
        particle.Play();
        yield return new WaitForSeconds(x);
        particle.Stop();
    }
}
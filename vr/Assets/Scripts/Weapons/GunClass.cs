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
    public bool canShoot = true;
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

    public void Stats(string newName, int newMaxClip, int newAmmoInClip, int newAmmo, float newReloadTime, float newShootingSpeed, float newBulletSpeed)
    {
        type = newName;
        ammo = newAmmo;
        maxClipSize = newMaxClip;
        ammoInClip = newAmmoInClip;
        reloadTime = newReloadTime;
        shootingSpeed = newShootingSpeed;
        bulletSpeed = newBulletSpeed;
    }

    public void Shooting()
    {
        if(ammoInClip >= 1 && canShoot)
        {
            StartCoroutine(ShootingSpeed(shootingSpeed));
        }    
        else if(ammoInClip <= 0)
        {
            StartCoroutine(Reload(reloadTime));
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
        bullet.GetComponent<Bullet>().thrust = bulletSpeed;
        bullet.GetComponent<Bullet>().Fired();
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
        Debug.Log("here");
        yield return new WaitForSeconds(x);
        Debug.Log("there");
        canShoot = true;
    }

    IEnumerator Reload(float x)
    {
        //ammo -= maxClipSize;
        yield return new WaitForSeconds(x);
        ammoInClip = maxClipSize;
    }

    IEnumerator MuzzleFlare(float x)
    {
        particle.Play();
        yield return new WaitForSeconds(x);
        particle.Stop();
    }
}
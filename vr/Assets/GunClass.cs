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
        if (ammoInClip <= 0)
        {
            StartCoroutine(Reload(reloadTime));
        }
        else if(ammoInClip >= 1 && canShoot)
        {
            StartCoroutine(ShootingSpeed(shootingSpeed));
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
        StartCoroutine(MuzzleFlare(shootingSpeed));
        canShoot = false;
        ammoInClip -= 1;
        yield return new WaitForSeconds(x);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : GunClass
{
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
        Stats("Handgun", 7, 7, 100, 1.2f, 0.2f, 80, 1f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunClass
{
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
        Stats("Shotgun", 2, 2, 12, 2, 0.1f, 80);
    }
}

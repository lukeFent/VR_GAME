using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunClass
{
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
        Stats("Shotgun", 1, 1, 12, 1.8f, 0, 80, 0.8f);
    }
}

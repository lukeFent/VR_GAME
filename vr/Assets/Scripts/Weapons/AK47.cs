using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : GunClass
{
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
        Stats("AK47", 20, 20, 120, 3, 0.08f, 120);
    }
}

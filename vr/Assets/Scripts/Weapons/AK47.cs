using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : GunClass
{
    private void Start()
    {
        anim = this.gameObject.GetComponent<Animation>();
        Stats("AK47", 20, 20, 120, 3.2f, 0.1f, 120, 2f);
    }
}

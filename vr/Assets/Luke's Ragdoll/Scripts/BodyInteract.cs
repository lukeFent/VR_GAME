using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInteract : InteractClass
{
    public RagDollController rag;

    public override void Interact()
    {
        rag.KnockAbout();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxChickens : Box, IBox
{
    public override void OpenBox()
    {
        base.OpenBox();
        ParticleSystem chickensPs = transform.Find("FX_Explosion_Chick").GetComponent<ParticleSystem>();
        chickensPs.Play();
    }
}

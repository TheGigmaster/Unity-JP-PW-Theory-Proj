using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCats : Box, IBox
{

    public override void OpenBox()
    {
        base.OpenBox();
        ParticleSystem catsPs = transform.Find("FX_Explosion_Cats").GetComponent<ParticleSystem>();
        catsPs.Play();
    }

}

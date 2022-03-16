using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDogs : Box, IBox
{
    public override void OpenBox()
    {
        base.OpenBox();
        ParticleSystem dogsPs = transform.Find("FX_Explosion_Dog").GetComponent<ParticleSystem>();
        dogsPs.Play();
    }

}

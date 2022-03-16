using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBox
{
    void SetAttributes(PhysicMaterial _boxBounce, Color _boxColor);
    void OpenBox();
    void Despawn();

}
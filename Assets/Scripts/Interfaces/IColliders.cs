using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColliders
{
    void OnHit(GameObject otherObject, float damage);
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MobilePhysics : MonoBehaviour
{
    // Max force that can be applied to a rigidbody
    protected const float MAX_VECTOR_FORCE = 200f;

    private void FixedUpdate()
    {
        Navigate();
    }

    protected abstract void Navigate();
    
}

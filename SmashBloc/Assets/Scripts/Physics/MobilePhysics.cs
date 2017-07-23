using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MobilePhysics : MonoBehaviour
{

    private void FixedUpdate()
    {
        Navigate();
    }

    protected abstract void Navigate();
    
}

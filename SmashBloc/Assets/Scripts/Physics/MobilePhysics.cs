using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This file will be expanded as physics methods common between Mobiles are 
 * identified. At the time of writing there is only one implemented Mobile, 
 * which is why this file seems barren.
 * **/
public abstract class MobilePhysics : MonoBehaviour
{
    // **         //
    // * FIELDS * //
    //         ** //

    // Max force that can be applied to a rigidbody
    protected const float MAX_VECTOR_FORCE = 200f;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// This method controls how the Mobile navigates. Normally this will 
    /// include functionality for steering forces.
    /// </summary>
    protected abstract void Navigate();

    private void FixedUpdate()
    {
        Navigate();
    }
}

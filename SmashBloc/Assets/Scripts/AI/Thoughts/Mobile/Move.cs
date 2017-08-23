using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Tells the Mobile to move to a destination. Mobiles will never head directly 
 * to a destination—they will deviate based on their DestDeviationRadius. This
 * is to simulate more realistic behavior.
 * **/
public class Move : MobileThought
{
    // **         //
    // * FIELDS * //
    //         ** //

    // Used to limit the precision with which Mobiles can move. Set to zero to
    // disable.
    private const float DEFAULT_DEVIATION_RADIUS = 20f;

    private Vector3 dest;
    private float deviationRadius;

    // **              //
    // * CONSTRUCTOR * //
    //              ** // 

    /// <summary>
    /// Note that the Y component of the "dest" vector will be nullified.
    /// </summary>
    public Move(Vector3 dest, float deviationRadius = DEFAULT_DEVIATION_RADIUS)
    {
        this.dest = new Vector3(dest.x, 0f, dest.z);
        this.deviationRadius = deviationRadius;
    }

    /// <summary>
    /// Note that this constructor does not posses a Y component parameter.
    /// </summary>
    public Move(float x, float z, float deviationRadius = DEFAULT_DEVIATION_RADIUS)
    {
        dest = new Vector3(x, 0f, z);
        this.deviationRadius = deviationRadius;
    }

    // **          //
    // * METHODS * //
    //          ** // 

    /// <summary>
    /// Constructs a 2D circle, picks a random point inside, and then adds the
    /// relevant values as a deviation to the Mobile's new destination.
    /// </summary>
    public override void Act()
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * deviationRadius;
        Vector3 newDest = new Vector3(dest.x + offset.x, dest.y, dest.z + offset.y);
        body.Destination = newDest;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Tells the Mobile to run away from a unit, usually an enemy, and also 
 * a default distance to run away.
 * **/
public class Flee : MobileThought
{
    // **         //
    // * FIELDS * //
    //         ** //

    // The default distance to flee.
    private const float DEFAULT_DISTANCE = 50f;

    // The unit to run away from.
    private Unit fleeFrom;
    // The distance to flee.
    private float distanceToFlee;

    // **              //
    // * CONSTRUCTOR * //
    //              ** //

    public Flee(Unit fleeFrom, float distanceToFlee = DEFAULT_DISTANCE)
    {
        this.fleeFrom = fleeFrom;
        this.distanceToFlee = distanceToFlee;
    }

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Calculates the direction to run away from and instructs the Body to 
    /// move toward that point.
    /// </summary>
    public override void Act()
    {
        Vector3 direction = body.transform.position - fleeFrom.transform.position;

        direction.y = 0; // so that it doesn't affect normalization
        // calculate the distance to run away
        direction = direction.normalized * distanceToFlee;

        // Tell the body to move AWAY from the general direction of the enemy
        body.Destination = body.transform.position - direction;
    }
}

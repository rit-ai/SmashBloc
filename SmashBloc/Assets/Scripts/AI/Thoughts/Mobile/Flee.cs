using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Tells the Mobile to run away from a group of GameObjects, usually enemy 
 * Units, and also provides a default distance to run away.
 * **/
public class FleeCommand : MobileThought
{
    // **         //
    // * FIELDS * //
    //         ** //

    // The default distance to flee.
    private const float DEFAULT_DISTANCE = 50f;

    // The objects to run away from.
    private List<GameObject> fleeFrom;
    // The distance to flee.
    private float distanceToFlee;

    // **              //
    // * CONSTRUCTOR * //
    //              ** //

    public FleeCommand(List<GameObject> fleeFrom, float distanceToFlee = DEFAULT_DISTANCE)
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
        Vector3 direction = Vector3.zero;

        // Get the vector that equals the general direction toward <g>
        foreach (GameObject g in fleeFrom)
        {
            direction += (body.transform.position - g.transform.position);
        }

        direction.y = 0; // so that it doesn't affect normalization
        // calculate the distance to run away
        direction = direction.normalized * distanceToFlee;

        // Tell the body to move AWAY from the general direction of the enemies
        body.Destination = body.transform.position - direction;
    }
}

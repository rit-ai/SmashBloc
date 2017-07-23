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
public class MoveCommand : MobileCommand
{
    private Vector3 dest;

    public MoveCommand(Vector3 dest)
    {
        this.dest = dest;
    }

    /// <summary>
    /// Constructs a 2D circle, picks a random point inside, and then adds the
    /// relevant values as a deviation to the Mobile's new destination.
    /// </summary>
    public override void Execute()
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * body.DestDeviationRadius;
        Vector3 newDest = new Vector3(dest.x + offset.x, dest.y, dest.z + offset.y);
        body.Destination = newDest;
    }
}

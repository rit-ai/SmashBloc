using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Sends a set of MobileUnits to a location.
 * 
 * Because of how this is set up, all units will attempt to move to the same
 * spot (this should be counterbalanced by flocking behaviors).
 * **/
public class SendMobilesToLoc : PlayerThought
{
    private List<MobileUnit> mobiles;
    private Vector3 location;

    public SendMobilesToLoc(List<MobileUnit> mobiles, Vector3 location)
    {
        this.mobiles = mobiles;
        this.location = location;
    }

    public override void Act()
    {
        foreach (MobileUnit mu in mobiles)
        {
            mu.PointOfInterest = location;
        }
    }
}

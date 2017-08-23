using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Sends a set of MobileUnits to a location by setting a new PointOfInterest.
 * **/
public class SendMobilesToLoc : PlayerThought
{
    // **         //
    // * FIELDS * //
    //         ** //

    private List<MobileUnit> mobiles;
    private Vector3 location;

    // **              //
    // * CONSTRUCTOR * //
    //              ** // 

    public SendMobilesToLoc(List<MobileUnit> mobiles, Vector3 location)
    {
        this.mobiles = mobiles;
        this.location = location;
    }

    // **          //
    // * METHODS * //
    //          ** // 

    public override void Act()
    {
        foreach (MobileUnit mu in mobiles)
        {
            mu.PointOfInterest = location;
        }
    }
}

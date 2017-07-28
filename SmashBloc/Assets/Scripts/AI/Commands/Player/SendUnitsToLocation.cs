using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Sends a set of MobileUnits to a location using a MoveCommand.
 * 
 * Because of how this is set up, all units will attempt to move to the same
 * spot (this should be counterbalanced by flocking behaviors).
 * **/
public class SendUnitsToLocation : PlayerCommand
{
    private List<MobileUnit> mobiles;
    private Vector3 location;

    public SendUnitsToLocation(List<MobileUnit> mobiles, Vector3 location)
    {
        this.mobiles = mobiles;
        this.location = location;
    }

    public override void Execute()
    {
        MoveCommand mc = new MoveCommand(location);
        foreach (MobileUnit mu in mobiles)
        {
            mc.Body = mu;
            mc.Execute();
        }
    }
}

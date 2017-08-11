using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Commands a MobileUnit to attack another Unit. Longer aim times will be more
 * accurate, but since Units stop while they take aim, they will also be more 
 * vulnerable to enemy fire.
 * **/
public class ShootCommand : MobileThought
{
    // **         //
    // * FIELDS * //
    //         ** //

    // The Unit we want to shoot.
    Unit target;
    // The max amount of time we're allowed to aim.
    float maxAimTime;

    // **              //
    // * CONSTRUCTOR * //
    //              ** // 

    public ShootCommand(Unit target, float maxAimTime)
    {
        this.target = target;
        this.maxAimTime = maxAimTime;
    }

    // **          //
    // * METHODS * //
    //          ** // 

    public override void Act()
    {
        body.Shoot(target, maxAimTime);
    }
}

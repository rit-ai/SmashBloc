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
public class Shoot : MobileThought
{
    // **         //
    // * FIELDS * //
    //         ** //

    const float DEFAULT_AIMTIME = 1f;

    // The Unit we want to shoot.
    Unit target;
    // The max amount of time we're allowed to aim.
    float aimTime;

    // **              //
    // * CONSTRUCTOR * //
    //              ** // 

    public Shoot(Unit target)
    {
        this.target = target;
        aimTime = DEFAULT_AIMTIME;
    }

    // **          //
    // * METHODS * //
    //          ** // 

    public override void Act()
    {
        body.Shoot(target, aimTime);
    }
}

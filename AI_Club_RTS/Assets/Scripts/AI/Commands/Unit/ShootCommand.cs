using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Commands a MobileUnit to attack another Unit. Longer aim times will be more
 * accurate, but as Units stop while they take aim, 
 * **/
public class ShootCommand : MobileCommand
{

    Unit target;
    float maxAimTime;

    public ShootCommand(Unit target, float maxAimTime)
    {
        this.target = target;
        this.maxAimTime = maxAimTime;
    }

    public override void Execute()
    {
        body.Shoot(target, maxAimTime);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * @author Paul Galatic
 * 
 * CLASS REQUIREMENTS:
 *      1) The Infantry will always travel to its absolute Destination, denoted
 *      by the Destination field in Unit.cs. However, it should also have state
 *      to "idle" around its absolute destination.
 *      2) If a unit is close enough to a unit from another team, it should 
 *      attempt to move directly to its position (in an attempt to attack).
 * **/
public class BasicInfantryAI : UnitAI {

    public override void Start ()
    {
        base.Start();
        currentState = new IdleState();
    }

    public override void UpdateState(object info)
    {
        //Debug.Assert(info is EnvironmentInfo);
        //EnvironmentInfo state = (EnvironmentInfo)info;
    }

    public class IdleState : State
    {



        public void StateUpdate()
        {

        }

    }

    public class AggressiveState : State
    {


        public void StateUpdate()
        {

        }
    }
}

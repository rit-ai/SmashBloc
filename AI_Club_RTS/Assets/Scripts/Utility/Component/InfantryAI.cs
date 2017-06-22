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
 *      attempt to move to its location and attack.
 * **/
public class InfantryAI : UnitAI {

    public InfantryAI(Infantry parent)
    {
        m_Parent = parent;
    }

    public override void ComponentUpdate()
    {

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

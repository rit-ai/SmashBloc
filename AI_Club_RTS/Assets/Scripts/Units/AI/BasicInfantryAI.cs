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
public class BasicInfantryAI : BaseAI {

    // The current state of the AI. AIs can have any number of potential
    // states, but only one state can be active at a time. A AI's state handles
    // its frame-by-frame behavior—is it aggressively pursuing enemies, or is 
    // it running away? Is it regrouping, advancing, or retreating?
    protected State currentState;

    public BasicInfantryAI()
    {
        currentState = new IdleState();
    }

    public void ComponentUpdate()
    {
        currentState.StateUpdate();
    }

    public void UpdateState(EnvironmentInfo info)
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

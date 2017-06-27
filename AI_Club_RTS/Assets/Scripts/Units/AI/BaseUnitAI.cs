using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * @author Paul Galatic
 * 
 * Base class to represent the lower limit of what an AI is expected to 
 * perform. To be used as a "starter file" for those that wish to develop new
 * AI for units, rather than having to change an existing one (though the 
 * existing files may freely be used as examples).
 * **/
public class BaseUnitAI : BaseAI
{
    // The absolute destination of the unit, separate from the local 
    // destination (which this AI should freely change). This value is set by 
    // either the player, or by a "parent AI" that controls all the units.
    protected Vector3 absoluteDest;

    // The current state of the AI. AIs can have any number of potential
    // states, but only one state can be active at a time. A Unit's State 
    // handles its frame-by-frame behavior—is it aggressively pursuing enemies,
    // or is it running away? Is it regrouping, advancing, or retreating?
    protected State currentState;

    public BaseUnitAI()
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

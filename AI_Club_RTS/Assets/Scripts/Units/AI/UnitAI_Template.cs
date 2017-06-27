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
 * 
 * One difference this class has from the Player AI is that it can expect
 * to regularly receive limited information from its Unit, rather than having
 * access to the entire scope of the battlefield at once.
 * **/
public class UnitAI_Template : BaseAI
{
    // The absolute destination of the unit, separate from the local 
    // destination (which this AI should freely change). This value is set by 
    // either the player, or by a "parent AI" that controls all the units.
    protected Vector3 absoluteDest;

    public UnitAI_Template()
    {
        currentState = new IdleState();
    }

    public override void UpdateState(object info)
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

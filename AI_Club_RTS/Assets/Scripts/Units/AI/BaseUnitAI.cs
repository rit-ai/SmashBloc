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
public class BaseUnitAI : UnitAI
{

    public BaseUnitAI(Infantry parent)
    {

        currentState = new IdleState();
    }

    public override void ComponentUpdate()
    {
        currentState.StateUpdate();
    }

    public override void UpdateState(EnvironmentInfo info)
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

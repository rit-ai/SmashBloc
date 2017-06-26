using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Abstract class that represents data common between the AI of different 
 * units.
 * **/
public abstract class UnitAI : RTS_Component {

    // Protected fields

    // The absolute destination of the unit, separate from the local 
    // destination (which this AI should freely change). This value is set by 
    // either the player, or by a "parent AI" that controls all the units.
    protected Vector3 absoluteDest;
    // The current state of the Unit's AI. AIs can have any number of potential
    // states, but only one state can be active at a time. A Unit's State 
    // handles its frame-by-frame behavior—is it aggressively pursuing enemies,
    // or is it running away? Is it regrouping, advancing, or retreating?
    protected State currentState;

    /// <summary>
    /// Update the component every frame, mainly by executing whatever behavior
    /// is denoted by its current State (in StateUpdate()).
    /// </summary>
    public abstract void ComponentUpdate();

    /// <summary>
    /// At regular intervals, the AI must make a decision to update its current
    /// state.
    /// </summary>
    /// <param name="info">Any information that may be relevant to the unit in 
    /// order for it to make a more effective decision, stored inside an 
    /// EnvironmentInfo struct. </param>
    public abstract void UpdateState(EnvironmentInfo info);

}

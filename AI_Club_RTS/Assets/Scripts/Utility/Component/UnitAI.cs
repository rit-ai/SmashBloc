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
    // The unit's parent, for reference purposes
    protected Unit m_Parent;
    // The absolute destination of the unit, separate from the local 
    // destination (which this AI should freely changed)
    protected Vector3 absoluteDest;
    // The current state of the Unit's AI. AIs can have any number of potential
    // states, but only one state can be active at a time.
    protected State currentState;

    /// <summary>
    /// Update the AI, either maintaining its current behavior or adjusting it
    /// to suit the present goals of the unit or its team.
    /// </summary>
    public abstract void ComponentUpdate();

    /// <summary>
    /// What behavior to perform when the Player sets this Unit to have a new
    /// absolute destination (for example, tuning the unit's destination to be
    /// slightly offset from the absolute destination such that they all don't
    /// crowd around a single point).
    /// </summary>
    public abstract void OnDestChanged();

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Abstract class that represents data common between the AI of different 
 * types. For instance, every AI must have a current state. AIs generally 
 * transitions between states according to an internal state machine that is 
 * defined based on the particular implementation.
 * **/
public abstract class BaseAI : MonoBehaviour {

    // The current state of the AI. AIs can have any number of potential
    // states, but only one state can be active at a time. A AI's state handles
    // its frame-by-frame behavior—is it aggressively pursuing enemies, or is 
    // it running away? Is it regrouping, advancing, or retreating?
    protected State currentState;

    /// <summary>
    /// At regular intervals, the AI must make a decision to update its current
    /// state.
    /// </summary>
    /// <param name="info">Any information that may be relevant to the unit in 
    /// order for it to make a more effective decision, stored inside an info 
    /// struct. It is the responsibility of the implementer to make sure that 
    /// the struct is of the right type. </param>
    public abstract void UpdateState(object info);

}

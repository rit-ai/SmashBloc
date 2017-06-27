using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class representing the most basic form of Player AI and providing details on
 * how to implement a custom one. At its core, a Player AI must be able to 
 * make decisions as to how to generate units, and also be able to decide where
 * to move those units.
 * **/
public abstract class AI_Player : Player, BaseAI {

    // The absolute destination of the unit, separate from the local 
    // destination (which this AI should freely change). This value is set by 
    // either the player, or by a "parent AI" that controls all the units.
    protected Vector3 absoluteDest;

    // The current state of the AI. AIs can have any number of potential
    // states, but only one state can be active at a time. A Unit's State 
    // handles its frame-by-frame behavior—is it aggressively pursuing enemies,
    // or is it running away? Is it regrouping, advancing, or retreating?
    protected State currentState;

    // The State will be reevaluated at this rate. Lower is faster, but bear
    // in mind that the UpdateState() function will likely have a high 
    // performance load if it is called more than once per second.
    private const float STATE_UPDATE_INTERVAL = 2.0f;


    /// <summary>
    /// Initializing the Team first because other functionality relies on it.
    /// This is bad code practice and should be fixed. FIXME.
    /// </summary>
    protected override void Awake()
    {
        // TODO random team generation
        team = new Team(this, "Toryteam", Color.red);
    }

    protected virtual new void Start () {
        base.Start();

        InvokeRepeating("ComponentUpdate", 0f, STATE_UPDATE_INTERVAL);

        absoluteDest = ownedCity.transform.position;
	}


    /// <summary>
    /// The AI player must decide which unit it wants to spawn, and where,
    /// based on its current state, which in turn is based on its 
    /// EnvironmentInfo.
    /// </summary>
    protected abstract void SetToSpawn();
    protected abstract void SetToSpawnAt();

    /// <summary>
    /// The AI player must be able to direct its units. What proportion of 
    /// units are directed where is decided by the implementation.
    /// </summary>
    protected abstract void SetNewDestination();

    // From interface
    public abstract void ComponentUpdate();

    // From interface
    public abstract void UpdateState(EnvironmentInfo info);
}

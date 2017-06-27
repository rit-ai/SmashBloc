using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class representing the most basic form of Player AI and providing details on
 * how to implement a custom one. At its core, a Player AI must be able to 
 * make decisions regarding:
 *  * Which units to generate
 *  * Where to generate those units
 * An AI should also direct its units, but that implementation is left open-
 * ended.
 * **/
public abstract class PlayerAI : BaseAI {

    // This is a reference to the body that the AI controls.
    protected Player body;

    public Player Body
    {
        set { body = value; }
    }

    /// <summary>
    /// The AI player must decide which unit it wants to spawn, and where,
    /// based on its current state, which in turn is based on its 
    /// EnvironmentInfo.
    /// </summary>
    protected abstract void SetToSpawn();
    protected abstract void SetToSpawnAt();

    // From interface
    public abstract override void UpdateState(object info);

    protected virtual void Start()
    {
        SetToSpawn();
        SetToSpawnAt();
    }
}

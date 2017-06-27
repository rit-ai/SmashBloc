using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The simplest form of master AI, this one only spawns Infantry units, only 
 * spawns them at its first city, and never moves its units. Naturally, this
 * is pretty boring—but it's a good example of behavior that should be 
 * implemented and extended in a better AI Player.
 * **/
public class PlayerAI_Basic : PlayerAI {

    protected override void Start () {
        base.Start();

        InvokeRepeating("Act", 0f, 1f);

        currentState = new IdleState(this);
	}

    /// <summary>
    /// Executes whatever behavior is denoted by the current State, and handles
    /// any other miscellaneous processing as well. This is called once every 
    /// second, instead of Update, which is called many times a second.
    /// </summary>
    public void Act()
    {
        currentState.StateUpdate();
    }

    public override void UpdateState(object info)
    {
         
    }

    protected override void SetToSpawn()
    {
        body.SetUnitToSpawn(Infantry.IDENTITY);
    }

    protected override void SetToSpawnAt()
    {
        Debug.Assert(body != null);
        body.SetCityToSpawnAt(body.Cities[0]);
    }

    private class IdleState : State
    {
        private PlayerAI_Basic brain;

        public IdleState(PlayerAI_Basic brain)
        {
            this.brain = brain;
        }

        public void StateUpdate()
        {
            brain.body.SpawnUnit();
        }
    }
}

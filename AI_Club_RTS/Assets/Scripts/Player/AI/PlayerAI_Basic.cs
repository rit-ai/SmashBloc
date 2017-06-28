using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The simplest form of master AI, this one only spawns Infantry units, only 
 * spawns them at its first city, and never moves its units. Naturally, this
 * is pretty boring,.
 * **/
public sealed class PlayerAI_Basic : PlayerAI {

    // Try to spawn a unit every SPAWN_UNIT_RATE seconds
    private const float SPAWN_UNIT_RATE = 5f;

    protected override void Start () {
        base.Start();

        currentState = new IdleState(this);
	}

    protected override void Decide()
    {
        
    }


    private class IdleState : State
    {
        private PlayerAI_Basic brain;

        public IdleState(PlayerAI_Basic brain)
        {
            this.brain = brain;

            brain.StartCoroutine(SpawnInfantry());
        }

        public void StateUpdate()
        {
        }

        private IEnumerator SpawnInfantry()
        {
            while (true)
            {
                while (brain.info == null) { yield return new WaitForSeconds(SPAWN_UNIT_RATE); }
                brain.AddCommand(new SpawnUnitCommand(brain.Body, Infantry.IDENTITY, brain.info.cities[0]));
                yield return new WaitForSeconds(SPAWN_UNIT_RATE);
            }
        }
    }
}

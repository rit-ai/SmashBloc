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
public sealed class PlayerAI_Basic : PlayerAI {

    // Try to spawn a unit every SPAWN_UNIT_RATE seconds
    private const float SPAWN_UNIT_RATE = 5f;

    

    private PlayerInfo workingInfo;

    protected override void Start () {
        base.Start();

        currentState = new IdleState(this);
	}

    public override void UpdateState(PlayerInfo info)
    {
        workingInfo = info;
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
                while (brain.workingInfo == null) { yield return SPAWN_UNIT_RATE; }
                brain.AddCommand(new SpawnUnitCommand(brain.Body, Infantry.IDENTITY, brain.workingInfo.cities[0]));
                yield return SPAWN_UNIT_RATE;
            }
        }
    }
}

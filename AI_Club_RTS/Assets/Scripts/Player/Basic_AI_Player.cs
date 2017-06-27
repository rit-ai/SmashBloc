using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_AI_Player : AI_Player {

    protected override void Start () {
        base.Start();

        SetToSpawn();
        SetToSpawnAt();
        currentState = new IdleState(this);
	}
	

    public override void ComponentUpdate()
    {
        currentState.StateUpdate();
    }

    public override void UpdateState(EnvironmentInfo info)
    {

    }

    protected override void SetToSpawn()
    {
        toSpawn = INFANTRY;
    }

    protected override void SetNewDestination()
    {
        absoluteDest = m_Cities[0].transform.position;
    }

    protected override void SetToSpawnAt()
    {
        toSpawnAt = m_Cities[0];
    }

    private class IdleState : State
    {
        private Basic_AI_Player player;

        public IdleState(Basic_AI_Player player)
        {
            this.player = player;
        }

        public void StateUpdate()
        {
            player.SpawnUnit();
        }
    }
}

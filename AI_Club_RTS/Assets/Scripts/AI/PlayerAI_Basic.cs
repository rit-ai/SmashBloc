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
    private const float SPAWN_UNIT_RATE = 3f;

    protected override void Start () {
        base.Start();
        StartCoroutine(SpawnInfantry());
    }

    protected override void Decide()
    {
        
    }

    private IEnumerator SpawnInfantry()
    {
        while (true)
        {
            while (info == null) { yield return new WaitForSeconds(SPAWN_UNIT_RATE); }
            foreach (City city in info.team.cities)
            {
                AddCommand(new SpawnUnitCommand(Infantry.IDENTITY, city));
            }
            yield return new WaitForSeconds(SPAWN_UNIT_RATE);
        }
    }

}

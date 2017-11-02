using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The simplest form of master AI.
 * 
 * It will generate units until it reaches a certain threshold. When and while 
 * that threshold is met, it will send its units to attack a target. It will 
 * find a new target to attack if its current one is destroyed. If its unit 
 * count is too low, it will move the units back to its one city.
 * **/
public sealed class PlayerAI_Basic : PlayerAI {

    // Unit count threshold to try an attack
    private const int ARMY_SIZE = 1;

    private City target;
    private int cooldown = 1;

    /// <summary>
    /// This AI attacks on a cooldown, since we want it to be able to spawn 
    /// units in the meantime between deciding whether or not to attack.
    /// </summary>
    protected override IThought Think()
    {
        if (target == null && info.team.enemies.Count > 0)
        {
            target = info.team.enemies[0].cities[0];
        }
        else if (info.team.enemies.Count == 0)
        {
            Debug.LogWarning("Could not find an enemy.");
        }

        if (cooldown <= 0 && info.team.mobiles.Count >= ARMY_SIZE && target != null)
        {
            // Take all the mobiles and tell them to move to an enemy city.
            // Presumably they will try to attack it.
            cooldown = ARMY_SIZE + 30;
            return new SendMobilesToLoc(info.team.mobiles, target.transform.position);
        }

        cooldown--;
        if (info.team.cities.Count > 0 && info.team.mobiles.Count < 3)
            return new SpawnMobile(Twirl.IDENTITY, info.team.cities[0]);

        return null;
    }

}

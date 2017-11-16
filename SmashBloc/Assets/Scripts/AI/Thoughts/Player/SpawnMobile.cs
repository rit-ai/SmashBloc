using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This command tells a Player to attempt to spawn a specific type of unit at
 * a specific city.
 * **/
public class SpawnMobile : PlayerThought {

    // **         //
    // * FIELDS * //
    //         ** //

    // The identity of the Mobile we're trying to spawn.
    string toSpawnName;
    // The city we're going to spawn it at.
    City toSpawnAt;

    // **              //
    // * CONSTRUCTOR * //
    //              ** // 

    public SpawnMobile(string toSpawnName, City toSpawnAt)
    {
        this.toSpawnName = toSpawnName;
        this.toSpawnAt = toSpawnAt;
    }

    // **          //
    // * METHODS * //
    //          ** //

    public override void Act()
    {
        // Cancel trying to spawn a unit at a city we don't own
        if (toSpawnAt.Team != body.Team) {
            Debug.LogWarning(body.name + " tried to spawn a unit at a city they didn't own.");
            return;
        }

        body.SetUnitToSpawn(toSpawnName);
        body.SetCityToSpawnAt(toSpawnAt);
        body.SpawnUnit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This command tells a Player to attempt to spawn a specific type of unit at
 * a specific city.
 * **/
public class SpawnUnit : PlayerThought {

    string toSpawnName;
    City toSpawnAt;

    public SpawnUnit(string toSpawnName, City toSpawnAt)
    {
        this.toSpawnName = toSpawnName;
        this.toSpawnAt = toSpawnAt;
    }

    public override void Act()
    {
        if (toSpawnAt.Team != body.Team) { return; }

        body.SetUnitToSpawn(toSpawnName);
        body.SetCityToSpawnAt(toSpawnAt);
        body.SpawnUnit();
    }
}

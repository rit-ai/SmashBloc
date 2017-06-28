using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This command tells a Player to attempt to spawn a specific type of unit at
 * a specific city.
 * **/
public class SpawnUnitCommand : PlayerCommand {

    public static string IDENTITY = "SpawnUnitCommand";

    string toSpawnName;
    City toSpawnAt;

    public SpawnUnitCommand(string toSpawnName, City toSpawnAt)
    {
        this.toSpawnName = toSpawnName;
        this.toSpawnAt = toSpawnAt;
    }

    public override void Execute()
    {
        body.SetUnitToSpawn(toSpawnName);
        body.SetCityToSpawnAt(toSpawnAt);
        body.SpawnUnit();
    }
}

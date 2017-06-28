using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitCommand : PlayerCommand {

    string toSpawnName;
    City toSpawnAt;

    public SpawnUnitCommand(Player body, string toSpawnName, City toSpawnAt) : base(body)
    {
        this.toSpawnName = toSpawnName;
        this.toSpawnAt = toSpawnAt;
    }

    public override void Execute()
    {
        Debug.Log("Yes");
        body.SetUnitToSpawn(toSpawnName);
        body.SetCityToSpawnAt(toSpawnAt);
        body.SpawnUnit();
    }
}

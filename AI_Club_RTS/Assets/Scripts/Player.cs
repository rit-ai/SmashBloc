using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * @author Paul Galatic
 * 
 * Class designed to handle all state encapsulated in a Player, such as name,
 * current number of units, current amount of gold, et cetera.
 */

public class Player : MonoBehaviour {

    private const int GOLD_INCREMENT_RATE = 2; // scale by which gold updates
    private const int MAX_GOLD_AMOUNT = 999; // richness ceiling
    private const int UNIT_GOLD_COST = 5; // gold cost for unit

    private int currentGoldAmount;
    private int currentUnits;

    private Unit unitToSpawn;

    // Use this for initialization
    void Start () {
        currentGoldAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateGold();
        Verify();
	}

    /// <summary>
    /// Sets the unit to spawn.
    /// </summary>
    public void SetUnitToSpawn(Unit unit)
    {
        unitToSpawn = unit;
    }

    /// <summary>
    /// Spawns a unit based on unitToSpawn.
    /// </summary>
    /// <param name="spawner">The spawner at which to spawn the unit.</param>
    public void SpawnUnit(Spawner spawner)
    {
        if (!(currentGoldAmount < unitToSpawn.Cost))
        {
            currentGoldAmount -= unitToSpawn.Cost;
            currentUnits++;
            spawner.Spawn(unitToSpawn);
        }
    }

    /// <summary>
    /// Returns the player's current amount of gold.
    /// </summary>
    public int GetGold()
    {
        return currentGoldAmount;
    }

    /// <summary>
    /// Returns the player's current amount of gold.
    /// </summary>
    public int GetUnits()
    {
        return currentUnits;
    }

    /// <summary>
    /// Modifies the current amount of gold.
    /// </summary>
    /// <param name="amount">The amount to modify by.</param>
    public void UpdateGold(int amount)
    {
        currentGoldAmount += amount;
    }

    /// <summary>
    /// Updates the current gold amount, reflecting passive gold gain.
    /// </summary>
    /// 
    private void UpdateGold()
    {
        var time = Time.deltaTime;
        if (time > 1 / GOLD_INCREMENT_RATE)
        {
            currentGoldAmount++;
        }
    }

    /// <summary>
    /// Cleans up any loose ends left by Update()'s sequential operation.
    /// </summary>
    private void Verify()
    {
        if (currentGoldAmount > MAX_GOLD_AMOUNT)
        {
            currentGoldAmount = MAX_GOLD_AMOUNT;
        }
    }
}

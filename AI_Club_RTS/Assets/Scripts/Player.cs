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

    // Public constants
    // Types of units a Player can own
    public Artillery ARTILLERY;
    public Bazooka BAZOOKA;
    public Infantry INFANTRY;
    public Recon RECON;
    public SupplyTruck SUPPLY_TRUCK;
    public Tank TANK;

    // Private Constants
    private const int GOLD_INCREMENT_RATE = 2;  // scale by which gold updates
                                                // --higher is faster

    private const int MAX_GOLD_AMOUNT = 999; // richness ceiling

    // Fields
    private Unit toSpawn;
    private int currentGoldAmount;
    private int currentUnits;

    // Use this for initialization
    void Start () {
        // Handle public constants
        //ARTILLERY =     Instantiate(ARTILLERY)      as Artillery;
        //BAZOOKA =       Instantiate(BAZOOKA)        as Bazooka;
        INFANTRY =      Instantiate(INFANTRY)       as Infantry;
        //RECON =         Instantiate(RECON)          as Recon;
        //SUPPLY_TRUCK =  Instantiate(SUPPLY_TRUCK)   as SupplyTruck;
        TANK =          Instantiate(TANK)           as Tank;

        currentGoldAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateGold();
        Verify();
	}

    /// <summary>
    /// Sets the unit to spawn. Throws an exception on an invalid name being 
    /// passed.
    /// </summary>
    /// <param name="unitName">The name of the unit to spawn, based on 
    /// Unit.NAME.</param>
    public void SetUnitToSpawn(string unitName)
    {
        switch (unitName)
        {
            case Artillery.NAME:
                toSpawn = ARTILLERY;
                break;
            case Bazooka.NAME:
                toSpawn = BAZOOKA;
                break;
            case Infantry.NAME:
                toSpawn = INFANTRY;
                break;
            case Recon.NAME:
                toSpawn = RECON;
                break;
            case SupplyTruck.NAME:
                toSpawn = SUPPLY_TRUCK;
                break;
            case Tank.NAME:
                toSpawn = TANK;
                break;
            default:
                throw new KeyNotFoundException("SetUnitToSpawn given invalid string");
        }
    }

    /// <summary>
    /// Spawns a unit based on unitToSpawn.
    /// </summary>
    /// <param name="spawner">The spawner at which to spawn the unit.</param>
    public void SpawnUnit(Spawner spawner)
    {
        if (!(currentGoldAmount < toSpawn.Cost))
        {
            currentGoldAmount -= toSpawn.Cost;
            currentUnits++;
            spawner.Spawn(toSpawn);
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

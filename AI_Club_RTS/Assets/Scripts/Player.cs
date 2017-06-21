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

    // Private Constants
    private const float GOLD_INCREMENT_RATE = 0.1f; // higher is slower

    private const int MAX_GOLD_AMOUNT = 999; // richness ceiling

    // Public fields
    // Types of units a Player can own
    public Artillery ARTILLERY;
    public Bazooka BAZOOKA;
    public Infantry INFANTRY;
    public Recon RECON;
    public SupplyTruck SUPPLY_TRUCK;
    public Tank TANK;

    public City defaultCity; // REMOVEME

    // Private fields
    private List<City> m_Cities;
    private List<Unit> m_Units;
    private Unit toSpawn;
    private int currentGoldAmount;
    private int currentNumUnits;

    // Use this for initialization
    void Start () {
        // Handle public constants


        // Handle fields
        m_Cities = new List<City>();
        m_Units = new List<Unit>();
        currentGoldAmount = 0;
        currentNumUnits = 0;

        // Handle function setup
        InvokeRepeating("UpdateGold", 0.0f, GOLD_INCREMENT_RATE);

        // Debug
        m_Cities.Add(defaultCity);
	}
	
	// Update is called once per frame
	void Update ()
    {

    }

    /// <summary>
    /// Sets the unit to spawn. Throws an exception on an invalid name being 
    /// passed.
    /// </summary>
    /// <param name="unitIdentity">The name of the unit to spawn, based on 
    /// Unit.NAME.</param>
    public void SetUnitToSpawn(string unitIdentity)
    {
        switch (unitIdentity)
        {
            case Artillery.IDENTITY:
                toSpawn = ARTILLERY;
                break;
            case Bazooka.IDENTITY:
                toSpawn = BAZOOKA;
                break;
            case Infantry.IDENTITY:
                toSpawn = INFANTRY;
                break;
            case Recon.IDENTITY:
                toSpawn = RECON;
                break;
            case SupplyTruck.IDENTITY:
                toSpawn = SUPPLY_TRUCK;
                break;
            case Tank.IDENTITY:
                toSpawn = TANK;
                break;
            default:
                throw new KeyNotFoundException("SetUnitToSpawn given invalid string");
        }

        
    }

    /// <summary>
    /// Spawns a unit based on toSpawn, if the Player has enough gold.
    /// </summary>
    public void SpawnUnit(City city)
    {
        if (currentGoldAmount > toSpawn.Cost)
        {
            Debug.Assert(toSpawn.Cost > 0);
            currentGoldAmount -= toSpawn.Cost;

            Unit newUnit = Utils.UnitToPrefab(toSpawn);
            Transform spawnPoint = city.SpawnPoint;
            newUnit = Instantiate(newUnit, spawnPoint.transform.position, Quaternion.identity);
            newUnit.setUnitName(newUnit.UnitName + currentNumUnits.ToString());
            m_Units.Add(newUnit);

            currentNumUnits++;
        }
    }

    /// <summary>
    /// Removes a unit from the list of units this player controls.
    /// </summary>
    /// <param name="unit">The unit to remove.</param>
    public void RemoveUnit(Unit unit)
    {
        m_Units.Remove(unit);
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
    /// Returns the player's current amount of gold.
    /// </summary>
    public int Gold
    {
        get { return currentGoldAmount; }
    }

    /// <summary>
    /// Returns the player's current amount of gold.
    /// </summary>
    public int NumUnits
    {
        get { return currentNumUnits; }
    }

    /// <summary>
    /// Updates the current gold amount, reflecting passive gold gain.
    /// </summary>
    /// 
    private void UpdateGold()
    {
        foreach (City c in m_Cities)
        {
            currentGoldAmount += c.IncomeLevel;
        }
        if (currentGoldAmount > MAX_GOLD_AMOUNT)
        {
            currentGoldAmount = MAX_GOLD_AMOUNT;
        }
    }

}

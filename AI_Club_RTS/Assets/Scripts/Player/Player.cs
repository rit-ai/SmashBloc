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
    private const float GOLD_INCREMENT_RATE = 0.3f; // higher is slower

    private const int MAX_GOLD_AMOUNT = 999; // richness ceiling

    // Public fields
    // Types of units a Player can own
    public Infantry INFANTRY;
    public Tank TANK;

    public City ownedCity;

    // Private fields
    protected List<City> m_Cities;
    protected List<Unit> m_Units;
    protected Unit toSpawn;
    protected City toSpawnAt;
    protected Team team;
    protected int currentGoldAmount;
    protected int currentNumUnits;

    /// <summary>
    /// Initializing the Team first because other functionality relies on it.
    /// This is bad code practice and should be fixed. FIXME.
    /// </summary>
    protected virtual void Awake()
    {
        team = new Team(this, "Dylanteam", Color.cyan);
    }

    // Use this for initialization
    protected virtual void Start () {
        // Handle private fields
        m_Cities = new List<City>();
        m_Units = new List<Unit>();
        currentGoldAmount = 0;
        currentNumUnits = 0;
        
        // Handle function setup
        InvokeRepeating("UpdateGold", 0.0f, GOLD_INCREMENT_RATE);

        // Debug FIXME
        ownedCity.Init(team);
        m_Cities.Add(ownedCity);
    }



    /// <summary>
    /// Sets the unit to spawn. Throws an exception on an invalid name being 
    /// passed.
    /// </summary>
    /// <param name="unitIdentity">The name of the unit to spawn, based on 
    /// Unit.NAME.</param>
    public virtual void SetUnitToSpawn(string unitIdentity)
    {
        switch (unitIdentity)
        {
            case Infantry.IDENTITY:
                toSpawn = INFANTRY;
                break;
            case Tank.IDENTITY:
                toSpawn = TANK;
                break;
            default:
                throw new KeyNotFoundException("SetUnitToSpawn given invalid string");
        }
    }

    /// <summary>
    /// Sets the city at which the next unit will be spawned.
    /// </summary>
    /// <param name="city"></param>
    public virtual void SetCityToSpawnAt(City city)
    {
        toSpawnAt = city;
    }

    /// <summary>
    /// Spawns a unit based on toSpawn, if the Player has enough gold.
    /// </summary>
    public virtual void SpawnUnit()
    {
        if (currentGoldAmount > toSpawn.Cost)
        {
            Debug.Assert(toSpawn.Cost > 0);
            currentGoldAmount -= toSpawn.Cost;

            Unit newUnit = Utils.UnitToPrefab(toSpawn);
            Transform spawnPoint = toSpawnAt.SpawnPoint;
            newUnit = Instantiate(newUnit, spawnPoint.transform.position, Quaternion.identity);
            newUnit.Init(team);
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
    /// Returns this player's team.
    /// </summary>
    public Team Team
    {
        get { return team; }
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
    protected void UpdateGold()
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

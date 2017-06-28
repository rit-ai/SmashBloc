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
    private const float PASS_INFO_RATE = 1f;
    private const float GOLD_INCREMENT_RATE = 0.1f; // higher is slower
    private const int MAX_GOLD_AMOUNT = 999; // richness ceiling
    private const int MAX_UNITS = 20;

    // Public fields
    // Types of units a Player can own
    public Infantry INFANTRY;
    public Tank TANK;

    // Debug
    public bool hasBrain;
    public City ownedCity;

    // Private fields
    private IEnumerator passInfo;
    private IEnumerator incrementGold;
    private PlayerAI brain;
    private Team team;
    private List<City> m_Cities;
    private List<Unit> m_Units;
    private Unit toSpawn;
    private City toSpawnAt;
    private int currentGoldAmount;
    private int currentNumUnits;

    /// <summary>
    /// Initializing the Team first because other functionality relies on it.
    /// This is bad code practice and should be fixed. FIXME.
    /// </summary>
    public virtual void Awake()
    {
        team = new Team(this, "Dylante", Color.cyan);

        if (hasBrain)
        {
            team = new Team(this, "AI_Team", Color.red);
            brain = gameObject.AddComponent<PlayerAI_Basic>();
            brain.Body = this;
        }
    }

    // Use this for initialization
    public virtual void Start () {
        // Handle private fields
        m_Cities = new List<City>();
        m_Units = new List<Unit>();
        currentGoldAmount = 0;
        currentNumUnits = 0;

        // Debug FIXME
        ownedCity.Init(team);
        m_Cities.Add(ownedCity);

        // Handle IEnumerators
        incrementGold = IncrementGold();
        StartCoroutine(incrementGold);

        if (hasBrain)
        {
            StartCoroutine(PassInfo());
        }
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
        if (currentNumUnits >= MAX_UNITS) { return; }

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
        set { team = value; }
    }

    /// <summary>
    /// Returns all of the cities this Player owns.
    /// </summary>
    public List<City> Cities
    {
        get { return m_Cities; }
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
    /// Passes PlayerInfo to this Player's brain.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PassInfo()
    {
        PlayerInfo info = new PlayerInfo();
        info.cities = m_Cities;
        brain.UpdateInfo(info);
        yield return PASS_INFO_RATE;
    }

    /// <summary>
    /// Updates the current gold amount, reflecting passive gold gain.
    /// </summary>
    private IEnumerator IncrementGold()
    {
        while (true)
        {
            foreach (City c in m_Cities)
            {
                currentGoldAmount += c.IncomeLevel;
            }
            if (currentGoldAmount > MAX_GOLD_AMOUNT)
            {
                currentGoldAmount = MAX_GOLD_AMOUNT;
            }
            yield return new WaitForSeconds(GOLD_INCREMENT_RATE);
        }
    }

}

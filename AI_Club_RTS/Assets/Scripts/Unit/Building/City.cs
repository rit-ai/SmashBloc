using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Ben Fairlamb
 * @author Paul Galatic
 * 
 * Class designed to handle City-specific functionality and state. Like with 
 * other game objects, menus and UI elements should be handled by observers.
 * **/
public class City : Unit {

    // Public constants
    public const string IDENTITY = "CITY";
    public const float MAX_HEALTH = 500f;
    public const int MAX_INCOME_LEVEL = 8;

    // Public fields
    public Transform m_SpawnPoint;

    // Private constants
    private const string DEFAULT_NAME = "Dylanto";
    // The health a city has just after it's captured
    private const float CAPTURED_HEALTH = 50f;
    // The rate at which a city's health regenerates
    private const float REGENERATE_HEALTH_RATE = 0.2f;
    private const int COST = 500;
    private const int MIN_INCOME_LEVEL = 1;
    private const int DEFAULT_INCOME_LEVEL = 8;

    // Private fields
    private List<IObserver> m_Observers;
    private string cityName;
    private int incomeLevel;

    /// <summary>
    /// Constructs and returns an instantiated and disabled City.
    /// </summary>
    public static City MakeCity()
    {
        City city = Instantiate(Toolbox.CityPrefab);
        city.gameObject.SetActive(false);

        return city;
    }

    // Initial state upon creation
    protected new void OnEnable()
    {
        maxHealth = MAX_HEALTH;
        // Default values
        incomeLevel = DEFAULT_INCOME_LEVEL;
        base.OnEnable();
    }

    /// <summary>
    /// What to do when the unit collides with another unit that's not on the 
    /// same team.
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null && !(unit.Team.Equals(team)))
        {
            TakeDamage(UnityEngine.Random.Range(10f, 20f), unit);
        }
    }

    /// <summary>
    /// Pull up the menu when the unit is clicked.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll(Invocation.CITY_MENU);
    }

    /// <summary>
    /// Causes the city to lose health. 
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    /// <param name="source">The source of the damage.</param>
    public override void TakeDamage(float damage, Unit source)
    {
        base.TakeDamage(damage, source);
    }

    /// <summary>
    /// If the city's health goes to or below 
    /// zero, its team changes to the team that caused the damage, and its 
    /// health gets set to a CAPTURED_HEALTH (to prevent rapid capturing / 
    /// recapturing in the event of a major skirmish).
    /// </summary>
    /// <param name="capturer">The capturer of the city.</param>
    protected override void OnDeath(Unit capturer)
    {
        health = CAPTURED_HEALTH;
        m_Surface.material.color = capturer.Team.color;
        cityName = capturer.Team.title;
        NotifyAll(Invocation.CITY_CAPTURED, capturer.Team);
    }

    /// <summary>
    /// Returns the location at which to spawn units.
    /// </summary>
    public Transform SpawnPoint
    {
        get { return m_SpawnPoint; }
    }

    /// <summary>
    /// Gets the Income Level of the city.
    /// </summary>
    public int IncomeLevel {
		get { return incomeLevel; }
	}

    /// <summary>
    /// Gets the name of the city.
    /// </summary>
    public string CityName
    {
        get
        {
            if (customName == null)
            {
                return cityName;
            }
            return customName;
        }
    }

    /// <summary>
    /// Returns the class name of the unit in the form of a string.
    /// </summary>
    public override string Identity()
    {
        return IDENTITY;
    }

    /// <summary>
    /// Returns how much a city would cost to place.
    /// </summary>
    public override int Cost()
    {
        return COST;
    }
}

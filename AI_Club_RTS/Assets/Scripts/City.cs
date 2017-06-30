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
public class City : MonoBehaviour, Observable {

    // Public constants
    public const float MAX_HEALTH = 500f;
    public const int MAX_INCOME_LEVEL = 8;
    public const int MIN_INCOME_LEVEL = 1;

    // Public fields
    public Transform m_SpawnPoint;
    public MeshRenderer m_HighlightInner;
    public MeshRenderer m_HighlightOuter;
    
    // Private constants
    private const string DEFAULT_NAME = "Dylanto";
    private const int DEFAULT_INCOME_LEVEL = 8;

    // Private fields
    private MeshRenderer m_Surface;
    private List<Observer> m_Observers;
    private Team team;
    private string cityName;
    private string customName;
    private float health;
    private int incomeLevel;

    // Use this for initialization
    void Start()
    {
        // Handle private fields
        m_Observers = new List<Observer>();
        m_Observers.Add(new UIObserver());

        // Default values
        health = MAX_HEALTH;
        incomeLevel = DEFAULT_INCOME_LEVEL;
        cityName = team.name;
    }

    /// <summary>
    /// Handles any data that needs to be passed after a city is instantiated.
    /// 
    /// TODO Until a player can spawn cities, this value is set by a public 
    /// variable.
    /// </summary>
    /// <param name="team">The Team the city is on when it is created.</param>
    public void Init(Team team)
    {
        m_Surface = GetComponent<MeshRenderer>();

        this.team = team;
        Debug.Assert(m_Surface != null);
        m_Surface.material.color = team.color;
    }

    /// <summary>
    /// Notifies all observers.
    /// </summary>
    /// <param name="data">The type of notification.</param>
    public void NotifyAll<T>(string invocation, params T[] data)
    {
        foreach (Observer o in m_Observers){
            o.OnNotify(this, invocation, data);
        }
    }

    /// <summary>
    /// What to do when the unit collides with another unit that's not on the 
    /// same team.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null && !(unit.Team.Equals(team)))
        {
            TakeDamage(UnityEngine.Random.Range(10f, 20f));
        }
        City city = collision.gameObject.GetComponent<City>();
        if (city != null && !(city.Team.Equals(team)))
        {
            TakeDamage(UnityEngine.Random.Range(10f, 20f));
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
    }

    /// <summary>
    /// Pull up the menu when the unit is clicked.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll<VoidObject>(UIObserver.INVOKE_CITY_DATA);
    }

    /// <summary>
    /// Highlights the unit.
    /// </summary>
    public void Highlight()
    {
        m_HighlightInner.enabled = true;
        m_HighlightOuter.enabled = true;
    }

    /// <summary>
    /// Removes highlighting on the unit.
    /// </summary>
    public void RemoveHighlight()
    {
        m_HighlightInner.enabled = false;
        m_HighlightOuter.enabled = false;
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

    public float Health { get { return health; } }

    /// <summary>
    /// Returns this city's team.
    /// </summary>
    public Team Team
    {
        get { return team; }
    }

    /// <summary>
    /// Sets the custom name of the city.
    /// </summary>
    public void SetCustomName(string newName)
    {
        customName = newName;
    }


}

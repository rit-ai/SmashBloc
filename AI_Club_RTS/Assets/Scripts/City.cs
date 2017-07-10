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
public class City : MonoBehaviour, IObservable {

    // Public constants
    public const float MAX_HEALTH = 500f;
    public const float CAPTURED_HEALTH = 50f;
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
    private List<IObserver> m_Observers;
    private Team team;
    private string cityName;
    private string customName;
    private float health;
    private int incomeLevel;

    /// <summary>
    /// Constructs and returns an instantiated City.
    /// </summary>
    /// <param name="team">The new City's Team.</param>
    /// <param name="prefab">The </param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static City MakeCity(Team team, City prefab, Vector3 position)
    {
        City city = Instantiate(prefab, position, Quaternion.identity);
        city.Team = team;
        city.m_Surface = city.GetComponent<MeshRenderer>();
        Debug.Assert(city.m_Surface != null);
        city.m_Surface.material.color = team.color;
        city.cityName = team.title;

        return city;
    }

    // Use this for initialization
    void Start()
    {
        // Handle private fields
        m_Observers = new List<IObserver>
        {
            gameObject.AddComponent<UIObserver>(),
            gameObject.AddComponent<GameObserver>()
        };

        // Default values
        health = MAX_HEALTH;
        incomeLevel = DEFAULT_INCOME_LEVEL;
    }

    /// <summary>
    /// Notifies all observers.
    /// </summary>
    /// <param name="data">The type of notification.</param>
    public void NotifyAll(Invocation invocation, params object[] data)
    {
        foreach (IObserver o in m_Observers){
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
            TakeDamage(Random.Range(10f, 20f), unit.Team);
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
    /// Causes the city to lose health. If the city's health goes to or below 
    /// zero, its team changes to the team that caused the damage, and its 
    /// health gets set to a CAPTURED_HEALTH (to prevent rapid capturing / 
    /// recapturing in the event of a major skirmish).
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    /// <param name="source">The source of the damage.</param>
    private void TakeDamage(float damage, Team source)
    {
        health -= damage;
        if (health < 0f)
        {
            health = CAPTURED_HEALTH;
            m_Surface.material.color = source.color;
            cityName = source.title;
            NotifyAll(Invocation.CITY_CAPTURED, source);
        }
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
    /// This city's team.
    /// </summary>
    public Team Team {
        get { return team; }
        set { team = value; }
    }

    /// <summary>
    /// Sets the custom name of the city.
    /// </summary>
    public void SetCustomName(string newName)
    {
        customName = newName;
    }


}

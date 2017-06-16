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
    public const int MAX_INCOME_LEVEL = 8;
    public const int MIN_INCOME_LEVEL = 1;

    // Public fields
    public Transform m_SpawnPoint;
    public Canvas m_HighlightCanvas;

    // Private constants
    private const string DEFAULT_NAME = "Dylanto";
    private const int DEFAULT_INCOME_LEVEL = 8;
    private static Quaternion SPAWN_ROTATION = Quaternion.FromToRotation(Vector3.up, Vector3.zero);

    // Private fields
    private List<Observer> m_Observers;
    // private string team;
    private string cityName;
    private string customName;
    private int incomeLevel;

    // Properties
    /// <summary>
    /// Gets the Team that currently owns the city.
    /// </summary>
    /// <value>The Team that owns the city.</value>
    //public string Team {
    //get { return team; }
    //}

    // Use this for initialization
    void Start()
    {
        // Handle private fields
        m_Observers = new List<Observer>();
        m_Observers.Add(new MenuObserver());

        incomeLevel = DEFAULT_INCOME_LEVEL;
        cityName = DEFAULT_NAME;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Notifies all observers.
    /// </summary>
    /// <param name="data">The type of notification.</param>
    public void NotifyAll<T>(T data)
    {
        foreach (Observer o in m_Observers){
            o.OnNotify(this, data);
        }
    }

    /// <summary>
    /// Pull up the menu when the unit is clicked.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll(MenuObserver.INVOKE_CITY_DATA);
    }

    /// <summary>
    /// Highlights the unit.
    /// </summary>
    public void Highlight()
    {
        m_HighlightCanvas.enabled = true;
    }

    /// <summary>
    /// Removes highlighting on the unit.
    /// </summary>
    public void RemoveHighlight()
    {
        m_HighlightCanvas.enabled = false;
    }

    /// <summary>
    /// Spawns the unit at the spawn point.
    /// </summary>
    public void SpawnUnit(Unit unit, int numUnits)
    {
        Unit newUnit = Utils.UnitToPrefab(unit);
        newUnit = Instantiate(newUnit, m_SpawnPoint.transform.position, SPAWN_ROTATION);
        newUnit.setUnitName(newUnit.UnitName + numUnits.ToString());
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
    /// Sets the custom name of the city.
    /// </summary>
    public void SetCustomName(string newName)
    {
        customName = newName;
    }


}

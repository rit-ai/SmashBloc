﻿using System.Collections;
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
    public MeshRenderer m_HighlightInner;
    public MeshRenderer m_HighlightOuter;
    public bool ownedByPlayer;
    
    // Private constants
    private const string DEFAULT_NAME = "Dylanto";
    private const int DEFAULT_INCOME_LEVEL = 8;

    // Private fields
    private MeshRenderer m_Surface;
    private List<Observer> m_Observers;
    private string team;
    private string cityName;
    private string customName;
    private int incomeLevel;

    // Use this for initialization
    void Start()
    {
        // Handle private fields
        m_Surface = GetComponent<MeshRenderer>();
        m_Observers = new List<Observer>();
        m_Observers.Add(new UIObserver());

        incomeLevel = DEFAULT_INCOME_LEVEL;
        cityName = DEFAULT_NAME;

        // Handle public fields
        if (ownedByPlayer)
        {
            m_Surface.material.color = Color.blue;
            team = Utils.PlayerTeamName;
        }
        else
        {
            // FIXME
            m_Surface.material.color = Color.red;
            team = "AI_TEAM";
        }


    }

    // Update is called once per frame
    void Update()
    {
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

    /// <summary>
    /// Returns this city's team.
    /// </summary>
    public string Team
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * @author Paul Galatic
 * 
 * Class designed to handle any UI-specific information that shows up on the
 * main overlay. Should **avoid** logic on what to display or what to pass
 * along if possible.
 */
public class UI_Manager : MonoBehaviour {

    // Private constants
    private const string CAMERA_NAME = "Main Camera";
    private const string SPAWNUNITBUTTON_NAME = "SpawnUnitButton";
    private const string GOLDAMOUNTTEXT_NAME = "GoldAmountText";
    private const string PLAYER_NAME = "Player";

    // Public fields
    // GENERAL
    public Camera m_Camera;
    // HEADER
    public Dropdown m_UnitSelectDropdown;
    public Text m_CurrentGoldAmountText;
    public Text m_CurrentUnitAmountText;
    // UNIT MENU
    public Canvas m_UnitMenuCanvas;
    public InputField m_UnitMenuNameInput;
    public Slider m_UnitMenuHealthSlider;
    // CITY MENU
    public Canvas m_CityMenuCanvas;
    public InputField m_CityMenuNameInput;
    public Slider m_CityMenuIncomeSlider;
    public Button m_CityMenuSpawnButton;

    // Private fields
    private Player m_Player;
    private Unit unitCurrentlyDisplayed;
    private City cityCurrentlyDisplayed;
    private Vector3 oldMousePos;

    // Initialize only once
    private void Awake()
    {
        // Set UI handlers
        // Handlers for changing a dropdown value
        m_UnitSelectDropdown.onValueChanged.AddListener(delegate { SetUnitToSpawn(); });
        // Handlers for finishing changing a name field
        m_UnitMenuNameInput.onEndEdit.AddListener(delegate { UpdateUnitName(); });
        m_UnitMenuNameInput.onEndEdit.AddListener(delegate { UpdateCityName(); });
        // Handlers for pressing a button on a menu
        m_CityMenuSpawnButton.onClick.AddListener(delegate { SpawnUnit(); });
    }

    // Initialize whenever this object loads
    void Start ()
    {
        // Handle private fields
        m_Player = GameObject.FindObjectOfType<Player>();
        m_UnitMenuCanvas.enabled = false;

        // Initialization
        SetUnitToSpawn();
	}

    /// <summary>
    /// Update the UI display.
    /// </summary>
	void Update ()
    {
        UpdateGoldAmountText();
        UpdateUnitAmountText();
        UpdateUnitMenu();

        oldMousePos = Input.mousePosition;
	}

    /// <summary>
    /// Sets the unit to spawn based on the value of the dropdown menu,
    /// and communicates that to Player.
    /// </summary>
    public void SetUnitToSpawn()
    {
        string toSpawn;
        switch (m_UnitSelectDropdown.value)
        {
            case 0:
                toSpawn = Infantry.IDENTITY;
                break;
            default:
                toSpawn = Tank.IDENTITY;
                break;
        }
        m_Player.SetUnitToSpawn(toSpawn);
    }

    /// <summary>
    /// Communicates that Player pressed the SpawnUnit button in a CityMenu.
    /// </summary>
    public void SpawnUnit()
    {
        m_Player.SpawnUnit(cityCurrentlyDisplayed);
    }

    /// <summary>
    /// Brings up the unit menu and displays unit's info. Handles any display
    /// info that won't require dynamic updating.
    /// </summary>
    /// <param name="unit">The unit whose info is to be displayed.</param>
    public void DisplayUnitInfo(Unit unit)
    {
        unitCurrentlyDisplayed = unit;
        //float damage = unitCurrentlyDisplayed.Damage;
        //float range = unitCurrentlyDisplayed.Range;
        //int cost = unitCurrentlyDisplayed.Cost;

        // Handle unit name input field
        m_UnitMenuNameInput.placeholder.GetComponent<Text>().text = unit.UnitName;

        // Handle health slider
        m_UnitMenuHealthSlider.maxValue = unit.MaxHealth;
        m_UnitMenuHealthSlider.value = unit.Health;
  
        // Once processing is finished, enable display
        m_UnitMenuCanvas.enabled = true;
    }

    /// <summary>
    /// Displays the city menu.
    /// </summary>
    /// <param name="city">The city to display.</param>
    public void DisplayCityInfo(City city)
    {
        cityCurrentlyDisplayed = city;

        // Handle city name input field
        m_CityMenuNameInput.placeholder.GetComponent<Text>().text = city.CityName;

        // Handle income slider
        m_CityMenuIncomeSlider.maxValue = City.MAX_INCOME_LEVEL;
        m_CityMenuIncomeSlider.value = city.IncomeLevel;

        // Once processing is finished, enable display
        m_CityMenuCanvas.enabled = true;
    }

    /// <summary>
    /// Updates the unit menu based on the dynamic status of the unit, if a 
    /// unit is being displayed.
    /// </summary>
    public void UpdateUnitMenu()
    {
        if (!m_UnitMenuCanvas.enabled) { return; }

        float health = unitCurrentlyDisplayed.Health;

        // Handle health slider
        m_UnitMenuHealthSlider.value = health;
    }

    /// <summary>
    /// Updates the city menu based on the dynamic status of the city, if a
    /// city is being displayed.
    /// </summary>
    private void UpdateCityInfo()
    {
        if (!m_CityMenuCanvas.enabled) { return; }

        int incomeLevel = cityCurrentlyDisplayed.IncomeLevel;

        // Handle income slider
        m_CityMenuIncomeSlider.value = incomeLevel;
    }

    /// <summary>
    /// Updates the currently displayed unit with a custom name.
    /// </summary>
    public void UpdateUnitName()
    {
        unitCurrentlyDisplayed.setCustomName(m_UnitMenuNameInput.text);
        Debug.Log(unitCurrentlyDisplayed.UnitName);
    }

    /// <summary>
    /// Updates the currently displayed city with a custom name.
    /// </summary>
    public void UpdateCityName()
    {
        cityCurrentlyDisplayed.SetCustomName(m_CityMenuNameInput.text);
    }

    /// <summary>
    /// Hides all currently displayed menus.
    /// </summary>
    public void CloseAll()
    {
        m_UnitMenuCanvas.enabled = false;
        m_CityMenuCanvas.enabled = false;
    }

    /// <summary>
    /// Moves a menu when the player drags it.
    /// </summary>
    /// <param name="menu">The menu to move.</param>
    public void MoveMenuOnDrag(Canvas menu)
    {
        Vector3 newMousePos = Input.mousePosition;
        Vector3 relativePos = newMousePos - oldMousePos;
        menu.transform.position += relativePos;
    }

    /// <summary>
    /// Updates the amount of gold a Player has in the overlay.
    /// </summary>
    private void UpdateGoldAmountText()
    {
        int gold = m_Player.GetGold();
        string goldText = gold.ToString();
        m_CurrentGoldAmountText.text = goldText;
    }

    /// <summary>
    /// Updates the number of units a Player has in the overlay.
    /// </summary>
    private void UpdateUnitAmountText()
    {
        int units = m_Player.GetUnits();
        string unitText = units.ToString();
        m_CurrentUnitAmountText.text = unitText;
    }


}

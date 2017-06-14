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
    public Button m_SpawnUnitButton;
    public Dropdown m_UnitSelectDropdown;
    public Text m_CurrentGoldAmountText;
    public Text m_CurrentUnitAmountText;
    // UNIT MENU
    public Canvas m_UnitMenuCanvas;
    public InputField m_UnitMenuNameInput;
    public Slider m_UnitMenuHealthSlider;

    // Private fields
    private Player m_Player;
    private Spawner m_Spawner;
    private Unit unitCurrentlyDisplayed;

    // Initialize only once
    private void Awake()
    {
        // Set handler for when the Player accesses the dropdown
        m_UnitSelectDropdown.onValueChanged.AddListener(delegate { SetUnitToSpawn(); });
        m_UnitMenuCanvas.enabled = false;
    }

    // Initialize whenever this object loads
    void Start ()
    {
        // Handle private fields
        m_Spawner = GameObject.FindObjectOfType<Spawner>();
        m_Player = GameObject.FindObjectOfType<Player>();

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
    /// Communicates that Player pressed the SpawnUnit button.
    /// </summary>
    public void SpawnUnit()
    {
        m_Player.SpawnUnit(m_Spawner);
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
        m_UnitMenuNameInput.placeholder.GetComponent<Text>().text = unit.name;
        m_UnitMenuNameInput.text = unit.Name;

        // Handle health slider
        m_UnitMenuHealthSlider.maxValue = unit.MaxHealth;
  
        // Once processing is finished, enable display
        m_UnitMenuCanvas.enabled = true;
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

    public void UpdateUnitName()
    {
        unitCurrentlyDisplayed.setCustomName(m_UnitMenuNameInput.text);
    }

    /// <summary>
    /// Hides the unit menu.
    /// </summary>
    public void HideUnitInfo()
    {
        m_UnitMenuCanvas.enabled = false;
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

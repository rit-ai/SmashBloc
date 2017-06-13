using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * @author Paul Galatic
 * 
 * Class designed to manage all UI-specific elements by communicating with 
 * any other classes that hold such information, particularly Player.
 */

public class UI_Manager : MonoBehaviour {

    private const string CAMERA_NAME = "Main Camera";
    private const string SPAWNUNITBUTTON_NAME = "SpawnUnitButton";
    private const string GOLDAMOUNTTEXT_NAME = "GoldAmountText";
    private const string PLAYER_NAME = "Player";

    public Camera m_Camera;
    public Button m_SpawnUnitButton;
    public Dropdown m_UnitSelectDropdown;
    public Text m_CurrentGoldAmountText;
    public Text m_CurrentUnitAmountText;

    private Player m_Player;
    private Spawner m_Spawner;

    private void Awake()
    {
        m_UnitSelectDropdown.onValueChanged.AddListener(delegate { SetUnitToSpawn(); });
    }

    void Start ()
    {
        m_Spawner = GameObject.FindObjectOfType<Spawner>();
        m_Player = GetComponentInParent<Player>();

        SetUnitToSpawn();
	}

	void Update ()
    {
        UpdateGoldText();
        UpdateUnitText();
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
                toSpawn = Infantry.NAME;
                break;
            default:
                toSpawn = Tank.NAME;
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
    /// Updates the amount of gold a Player has in the overlay.
    /// </summary>
    private void UpdateGoldText()
    {
        int gold = m_Player.GetGold();
        string goldText = gold.ToString();
        m_CurrentGoldAmountText.text = goldText;
    }

    /// <summary>
    /// Updates the number of units a Player has in the overlay.
    /// </summary>
    private void UpdateUnitText()
    {
        int units = m_Player.GetUnits();
        string unitText = units.ToString();
        m_CurrentUnitAmountText.text = unitText;
    }




}

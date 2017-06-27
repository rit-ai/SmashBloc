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
    private const float WAIT_TIME = 1f;

    // Public fields
    // GENERAL
    public Player m_Player;
    public Camera m_Camera;
    // HEADER
    public Dropdown m_UnitSelect;
    public Text m_CurrentGoldAmount;
    public Text m_CurrentUnitAmount;
    // STARTING AND ENDING GAME
    public Text m_StartMessage;
    // UNIT MENU
    public Canvas m_UnitMenu;
    public InputField m_UnitMenuNameInput;
    public Slider m_UnitMenuHealth;
    // CITY MENU
    public Canvas m_CityMenu;
    public InputField m_CityMenuNameInput;
    public Slider m_CityMenuIncome;
    public Button m_CityMenuSpawnButton;
    // MISC UI
    public GameObject m_TargetRing;

    // Private fields
    private Unit unitCurrentlyDisplayed;
    private City cityCurrentlyDisplayed;
    private Vector3 oldMousePos;
    private Vector3 menuSpawnPos;
    private IEnumerator coroutine;

    // Initialize only once
    private void Awake()
    {
        // Set UI handlers
        // Handlers for changing a dropdown value
        m_UnitSelect.onValueChanged.AddListener(delegate { SetUnitToSpawn(); });
        // Handlers for finishing changing a name field
        m_UnitMenuNameInput.onEndEdit.AddListener(delegate { UpdateUnitName(); });
        m_UnitMenuNameInput.onEndEdit.AddListener(delegate { UpdateCityName(); });
        // Handlers for pressing a button on a menu
        m_CityMenuSpawnButton.onClick.AddListener(delegate { SpawnUnit(); });

        // Set IEnumerators
        coroutine = AnimateStart(WAIT_TIME);
    }

    // Initialize whenever this object loads
    void Start ()
    {
        // Instantiate misc UI
        m_TargetRing = Instantiate(m_TargetRing);

        // Handle private fields
        menuSpawnPos = m_UnitMenu.transform.position;

        // Initialization
        StartCoroutine(coroutine);
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
        switch (m_UnitSelect.value)
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
        m_Player.SetCityToSpawnAt(cityCurrentlyDisplayed);
        m_Player.SpawnUnit();
    }

    /// <summary>
    /// Brings up the unit menu and displays unit's info. Handles any display
    /// info that won't require dynamic updating. Buttons will be disabled or
    /// enabled depending on whether or not the player owns that unit.
    /// </summary>
    /// <param name="unit">The unit whose info is to be displayed.</param>
    public void DisplayUnitInfo(Unit unit, bool enableCommand)
    {
        unitCurrentlyDisplayed = unit;
        //float damage = unitCurrentlyDisplayed.Damage;
        //float range = unitCurrentlyDisplayed.Range;
        //int cost = unitCurrentlyDisplayed.Cost;

        // Set position to wherever menus are supposed to appear
        m_UnitMenu.transform.position = menuSpawnPos;

        // Handle unit name input field
        m_UnitMenuNameInput.enabled = enabled;
        m_UnitMenuNameInput.placeholder.GetComponent<Text>().text = unit.UnitName;

        // Handle health slider
        m_UnitMenuHealth.maxValue = unit.MaxHealth;
        m_UnitMenuHealth.value = unit.Health;

        // Once processing is finished, bring to front and enable display
        m_UnitMenu.transform.SetAsLastSibling();
        m_UnitMenu.enabled = true;
    }

    /// <summary>
    /// Displays the city menu. Handles any display info that won't require 
    /// dynamic updating. Buttons will be disabled or enabled depending on 
    /// whether or not the player owns that unit. 
    /// </summary>
    /// <param name="city">The city to display.</param>
    public void DisplayCityInfo(City city, bool enabled)
    {
        cityCurrentlyDisplayed = city;

        // Set position to wherever menus are supposed to appear
        m_CityMenu.transform.position = menuSpawnPos;

        // Handle city name input field
        m_CityMenuNameInput.enabled = enabled;
        m_CityMenuNameInput.placeholder.GetComponent<Text>().text = city.CityName;

        // Handle spawn button
        m_CityMenuSpawnButton.enabled = enabled;

        // Handle income slider
        m_CityMenuIncome.maxValue = City.MAX_INCOME_LEVEL;
        m_CityMenuIncome.value = city.IncomeLevel;

        // Once processing is finished, bring to front and enable display
        m_CityMenu.transform.SetAsLastSibling();
        m_CityMenu.enabled = true;
    }

    /// <summary>
    /// Updates the unit menu based on the dynamic status of the unit, if a 
    /// unit is being displayed.
    /// </summary>
    public void UpdateUnitMenu()
    {
        if (!m_UnitMenu.enabled) { return; }

        float health = unitCurrentlyDisplayed.Health;

        // Handle health slider
        m_UnitMenuHealth.value = health;
    }

    /// <summary>
    /// Updates the city menu based on the dynamic status of the city, if a
    /// city is being displayed.
    /// </summary>
    private void UpdateCityInfo()
    {
        if (!m_CityMenu.enabled) { return; }

        int incomeLevel = cityCurrentlyDisplayed.IncomeLevel;

        // Handle income slider
        m_CityMenuIncome.value = incomeLevel;
    }

    /// <summary>
    /// Updates the currently displayed unit with a custom name.
    /// </summary>
    public void UpdateUnitName()
    {
        unitCurrentlyDisplayed.setCustomName(m_UnitMenuNameInput.text);
    }

    /// <summary>
    /// Updates the currently displayed city with a custom name.
    /// </summary>
    public void UpdateCityName()
    {
        cityCurrentlyDisplayed.SetCustomName(m_CityMenuNameInput.text);
    }

    /// <summary>
    /// Displays the target ring at the current mouse position.
    /// </summary>
    /// <param name="terrain"></param>
    public void DisplayTargetRing(RTS_Terrain terrain)
    {
        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, terrain.ignoreAllButTerrain))
        {
            m_TargetRing.transform.position = hit.point;
            m_TargetRing.GetComponent<Renderer>().enabled = true;
            foreach (Renderer r in m_TargetRing.GetComponentsInChildren<Renderer>())
            {
                r.enabled = true;
            }
        }
    }

    /// <summary>
    /// Hides all currently displayed menus.
    /// </summary>
    public void CloseAll()
    {
        m_TargetRing.GetComponent<Renderer>().enabled = false;
        foreach (Renderer r in m_TargetRing.GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
        m_UnitMenu.enabled = false;
        m_CityMenu.enabled = false;
    }

    /// <summary>
    /// Moves a menu, and brings it to the front, when the player drags it.
    /// </summary>
    /// <param name="menu">The menu to move.</param>
    public void MoveMenuOnDrag(Canvas menu)
    {
        menu.transform.SetAsLastSibling();
        Vector3 newMousePos = Input.mousePosition;
        Vector3 relativePos = newMousePos - oldMousePos;
        menu.transform.position += relativePos;
    }

    /// <summary>
    /// Updates the amount of gold a Player has in the overlay.
    /// </summary>
    private void UpdateGoldAmountText()
    {
        int gold = m_Player.Gold;
        string goldText = gold.ToString();
        m_CurrentGoldAmount.text = goldText;
    }

    /// <summary>
    /// Updates the number of units a Player has in the overlay.
    /// </summary>
    private void UpdateUnitAmountText()
    {
        int units = m_Player.NumUnits;
        string unitText = units.ToString();
        m_CurrentUnitAmount.text = unitText;
    }

    /// <summary>
    /// Animates the "start round" text of a game.
    /// </summary>
    /// <param name="waitTime">The amount of time to wait before playing the
    /// animation, in seconds.</param>
    private IEnumerator AnimateStart(float waitTime)
    {
        const int FRAMES_TO_LINGER = 90;
        const float MOVE_DISTANCE_LARGE = 11f;
        const float MOVE_DISTANCE_SMALL = 2f;
        const float MIN_DISTANCE_SQR = 36f;
        Color textColor = new Color(1f, 1f, 1f, 0f); // white, but invisible
        Vector3 textPosition = m_StartMessage.transform.position;
        textPosition.x = 0;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);

        yield return new WaitForSeconds(waitTime);

        m_StartMessage.enabled = true;
        //m_StartMessage.color = textColor;
        m_StartMessage.transform.position = textPosition;

        // Until the text is near the center of the screen, move it to the 
        // right and raise the alpha
        while ((m_StartMessage.transform.position - screenCenter).sqrMagnitude > MIN_DISTANCE_SQR)
        {
            textColor.a += 0.04f;
            textPosition.x += MOVE_DISTANCE_LARGE;
            m_StartMessage.color = textColor;
            m_StartMessage.transform.position = textPosition;
            yield return 0f;
        }

        // Let it linger for FRAMES_TO_LINGER frames
        for (int x = 0; x < FRAMES_TO_LINGER; x++)
        {
            textPosition.x += MOVE_DISTANCE_SMALL;
            m_StartMessage.transform.position = textPosition;
            yield return 0f;
        }

        // Until text is offscreen, move to the right and fade out
        while (m_StartMessage.transform.position.x < Screen.width * 1.5)
        {
            textColor.a -= 0.03f;
            textPosition.x += MOVE_DISTANCE_LARGE;
            m_StartMessage.color = textColor;
            m_StartMessage.transform.position = textPosition;
            yield return 0f;
        }

        m_StartMessage.enabled = false;

        yield return null;
    }


}

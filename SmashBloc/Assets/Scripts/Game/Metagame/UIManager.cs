using System;
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
public class UIManager : MonoBehaviour, IObservable {

    // Public constants
    public const string ATTACHED_TO = "Overlay";

    // Private constants
    private const string CAMERA_NAME = "Main Camera";
    private const string SPAWNUNITBUTTON_NAME = "SpawnUnitButton";
    private const string GOLDAMOUNTTEXT_NAME = "GoldAmountText";
    private const string PLAYER_NAME = "Player";
    private const float WAIT_TIME = 1f;

    // Public fields
    // GENERAL
    public Camera m_Camera;
    public Canvas m_PauseText;
    // MENU
    public Canvas m_PauseMenu;
    public Canvas m_DevMenu;
    public Button m_ResetButton;
    public Button m_OptionsButton;
    public Toggle m_OptionsMenuDevModeToggle;
    // HEADER
    public Dropdown m_UnitSelect;
    public Text m_CurrentGoldAmount;
    public Text m_CurrentUnitAmount;
    public Button m_DevMenuButton;
    // STARTING AND ENDING GAME
    public Text m_Message;
    // UNIT MENU
    public Canvas m_UnitMenu;
    public InputField m_UnitMenuNameInput;
    public Slider m_UnitMenuHealth;
    // CITY MENU
    public Canvas m_CityMenu;
    public InputField m_CityMenuNameInput;
    public Slider m_CityMenuHealth;
    public Slider m_CityMenuIncome;
    public Button m_CityMenuSpawnButton;
    // MISC UI
    public TargetRing m_TargetRing;

    // Private fields
    private List<IObserver> observers;
    private Unit unitCurrentlyDisplayed;
    private City cityCurrentlyDisplayed;
    private Vector3 oldMousePos;
    private Vector3 menuSpawnPos;
    private bool inOptionsMenu = false;

    public void NotifyAll(Invocation invoke, params object[] data)
    {
        foreach (IObserver o in observers)
        {
            o.OnNotify(this, invoke, data);
        }
    }

    /// <summary>
    /// Toggles whether or not the pause menu is visible. And will toggle back
    /// to a root menu, if in sub-menu
    /// </summary>
    public void ToggleMenu()
    {
        if(inOptionsMenu)
        {
            ToggleOptionsMenu();
        }
        else
        {
            m_PauseMenu.enabled = !(m_PauseMenu.enabled);
            m_PauseMenu.transform.SetAsLastSibling();
        }
        
    }

    /// <summary>
    /// Toggles whether or not the buttons related to the options menu are
    /// visible. Will also notify observers of current menu state
    /// </summary>
    public void ToggleOptionsMenu()
    {
        // Toggling buttons on/off and the inOptionsMenu boolean
        inOptionsMenu = !inOptionsMenu;
        m_ResetButton.gameObject.SetActive(!m_ResetButton.gameObject.activeSelf);
        m_OptionsButton.gameObject.SetActive(!m_OptionsButton.gameObject.activeSelf);
        m_OptionsMenuDevModeToggle.gameObject.SetActive(!m_OptionsMenuDevModeToggle.gameObject.activeSelf);
        // Notifies all observers of the current menu state
        if (!inOptionsMenu)
        {
            NotifyAll(Invocation.IN_MAINMENU);
        }
        else
        {
            NotifyAll(Invocation.IN_SUBMENU);
        }
    }

    /// <summary>
    /// Toggles whether or not the pause text is visible.
    /// </summary>
    public void TogglePauseText()
    {
        m_ResetButton.enabled = !(m_ResetButton);
        m_ResetButton.enabled = !(m_ResetButton.enabled);
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
                toSpawn = Twirl.IDENTITY;
                break;
            default:
                toSpawn = Boomy.IDENTITY;
                break;
        }
        GameManager.PLAYER.SetUnitToSpawn(toSpawn);
    }

    /// <summary>
    /// Communicates that Player pressed the SpawnUnit button in a CityMenu.
    /// </summary>
    public void SpawnUnit()
    {
        GameManager.PLAYER.SetCityToSpawnAt(cityCurrentlyDisplayed);
        GameManager.PLAYER.SpawnUnit();
    }

    /// <summary>
    /// Brings up the unit menu and displays unit's info. Handles any display
    /// info that won't require dynamic updating. Buttons will be disabled or
    /// enabled depending on whether or not the player owns that unit.
    /// </summary>
    /// <param name="unit">The unit whose info is to be displayed.</param>
    public void DisplayUnitInfo(MobileUnit unit, bool enableCommand)
    {
        // Only allow one highlighting ring
        if (unitCurrentlyDisplayed != null && unitCurrentlyDisplayed != unit)
            unitCurrentlyDisplayed.RemoveHighlight();

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
        // Only allow one highlighting ring
        if (cityCurrentlyDisplayed != null && cityCurrentlyDisplayed != city)
            cityCurrentlyDisplayed.RemoveHighlight();

        cityCurrentlyDisplayed = city;

        // Set position to wherever menus are supposed to appear
        m_CityMenu.transform.position = menuSpawnPos;

        // Handle city name input field
        m_CityMenuNameInput.enabled = enabled;
        m_CityMenuNameInput.placeholder.GetComponent<Text>().text = city.UnitName;

        // Handle spawn button
        m_CityMenuSpawnButton.enabled = enabled;

        // Handle sliders
        m_CityMenuHealth.maxValue = City.MAX_HEALTH;
        m_CityMenuHealth.value = city.Health;
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

        // Handle health slider
        m_UnitMenuHealth.value = unitCurrentlyDisplayed.Health;
    }

    /// <summary>
    /// Animates the "start round" text of a game.
    /// </summary>
    /// <param name="waitTime">The amount of time to wait before playing the
    /// animation, in seconds.</param>
    public IEnumerator AnimateText(string text)
    {
        // Don't animate if we're already animating something
        if (m_Message.enabled) { yield break; }

        const int FRAMES_TO_LINGER = 60;
        const float MOVE_DISTANCE_LARGE = 15f;
        const float MOVE_DISTANCE_SMALL = 2f;
        const float MIN_DISTANCE_SQR = 30000f;
        Color textColor = new Color(1f, 1f, 1f, 0f); // white, but invisible
        Vector3 textPosition = m_Message.transform.position;
        textPosition.x = 0;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);

        yield return null;

        m_Message.text = text;
        m_Message.color = textColor;
        m_Message.transform.position = textPosition;
        m_Message.enabled = true;

        // Until the text is near the center of the screen, move it to the 
        // right and raise the alpha
        while ((m_Message.transform.position - screenCenter).sqrMagnitude > MIN_DISTANCE_SQR)
        {
            textColor.a += 0.10f;
            textPosition.x += MOVE_DISTANCE_LARGE;
            m_Message.color = textColor;
            m_Message.transform.position = textPosition;
            yield return null;
        }

        // Let it linger for FRAMES_TO_LINGER frames
        for (int x = 0; x < FRAMES_TO_LINGER; x++)
        {
            textPosition.x += MOVE_DISTANCE_SMALL;
            m_Message.transform.position = textPosition;
            yield return null;
        }

        // Until text is offscreen, move to the right and fade out
        while (m_Message.transform.position.x < Screen.width * 1.5)
        {
            textColor.a -= 0.05f;
            textPosition.x += MOVE_DISTANCE_LARGE;
            m_Message.color = textColor;
            m_Message.transform.position = textPosition;
            yield return null;
        }

        m_Message.enabled = false;
        NotifyAll(Invocation.ANIMATION_FINISHED);
    }

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
        m_ResetButton.onClick.AddListener(delegate { ResetButtonPressed(); });
        m_OptionsButton.onClick.AddListener(delegate { OptionsButtonPressed(); });
        m_DevMenuButton.onClick.AddListener(delegate { DevButtonPressed(); });
        //Handlers for pressing a toggle
        m_OptionsMenuDevModeToggle.onValueChanged.AddListener((e) => { DevToggleListener(e); });
    }

    // Initialize whenever this object loads
    void Start ()
    {
        observers = new List<IObserver>
        {
            gameObject.AddComponent<GameObserver>()
        };

        // Hide menus / messages
        CloseAll();

        // Instantiate misc UI
        m_TargetRing = Instantiate(m_TargetRing);

        // Handle private fields
        menuSpawnPos = m_UnitMenu.transform.position;

        // Initialization
        SetUnitToSpawn();
        
        m_DevMenuButton.gameObject.SetActive(false);

    }

    /// <summary>
    /// Update the UI display.
    /// </summary>
	void Update ()
    {
        UpdateGoldAmountText();
        UpdateUnitAmountText();
        UpdateUnitMenu();
        UpdateCityMenu();

        oldMousePos = Input.mousePosition;
	}

    /// <summary>
    /// Announces that the reset button was pressed.
    /// </summary>
    private void ResetButtonPressed()
    {
        CloseAll();
        NotifyAll(Invocation.RESET_GAME);
    }

    /// <summary>
    /// Calls ToggleOptionsMenu()
    /// </summary>
    private void OptionsButtonPressed()
    {
        ToggleOptionsMenu();
    }

    /// <summary>
    /// Causes the devmenu to show up
    /// </summary>
    private void DevButtonPressed()
    {
        m_DevMenu.enabled = !(m_DevMenu.enabled);
    }

    /// <summary>
    /// Updates the city menu based on the dynamic status of the city, if a
    /// city is being displayed.
    /// </summary>
    private void UpdateCityMenu()
    {
        if (!m_CityMenu.enabled) { return; }

        // Handle sliders
        m_CityMenuHealth.value = cityCurrentlyDisplayed.Health;
        m_CityMenuIncome.value = cityCurrentlyDisplayed.IncomeLevel;
    }

    /// <summary>
    /// Updates the currently displayed unit with a custom name.
    /// </summary>
    public void UpdateUnitName()
    {
        unitCurrentlyDisplayed.SetCustomName(m_UnitMenuNameInput.text);
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
    /// <param name="terrain">The GameObject currently serving as the ground.</param>
    public void DisplayTargetRing(RTS_Terrain terrain)
    {
        RaycastHit hit;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, terrain.ignoreAllButTerrain))
        {
            m_TargetRing.UpdatePosition(hit.point);
            m_TargetRing.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the target ring.
    /// </summary>
    public void HideTargetRing()
    {
        m_TargetRing.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hides all currently displayed menus.
    /// </summary>
    public void CloseAll()
    {
        if(Time.timeScale != 0)
        {
            m_TargetRing.gameObject.SetActive(false);
            m_PauseText.enabled = false;
            m_PauseMenu.enabled = false;
            m_DevMenu.enabled = false;
            m_UnitMenu.enabled = false;
            m_CityMenu.enabled = false;
            m_Message.enabled = false;
        }
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
    /// Enables or disables the DevMenuButton
    /// </summary>
    /// /// <param name="isOn">boolean value that determines if the button 
    /// should be visible</param>
    private void DevToggleListener(bool isOn)
    {
        m_DevMenuButton.gameObject.SetActive(isOn);
    }

    /// <summary>
    /// Updates the amount of gold a Player has in the overlay.
    /// </summary>
    private void UpdateGoldAmountText()
    {
        int gold = GameManager.PLAYER.Gold;
        string goldText = gold.ToString();
        m_CurrentGoldAmount.text = goldText;
    }

    /// <summary>
    /// Updates the number of units a Player has in the overlay.
    /// </summary>
    private void UpdateUnitAmountText()
    {
        int units = GameManager.PLAYER.Team.mobiles.Count;
        string unitText = units.ToString();
        m_CurrentUnitAmount.text = unitText;
    }


}

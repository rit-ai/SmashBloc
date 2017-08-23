using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @author Paul Galatic
 * 
 * Class designed to handle any UI-specific information that shows up on the
 * main overlay. Should **avoid** logic on what to display or what to pass
 * along if possible.
 * **/
public class UIManager : MonoBehaviour, IObservable
{
    // **         //
    // * FIELDS * //
    //         ** //

    public const string ATTACHED_TO = "Overlay";

    // GENERAL
    public Camera cam;
    public Canvas pauseText;
    // MENU
    public Canvas pauseMenu;
    public Button resetButton;
    public Button exitButton;
    // HEADER
    public Dropdown unitSelect;
    public Text currentGoldAmount;
    public Text currentUnitAmount;
    // STARTING AND ENDING GAME
    public Text message;
    // UNIT MENU
    public Canvas unitMenu;
    public InputField unitMenuName;
    public Slider unitMenuHealth;
    // CITY MENU
    public Canvas cityMenu;
    public InputField cityMenuName;
    public Slider cityMenuHealth;
    public Slider cityMenuIncome;
    public Button cityMenuSpawn;
    // MISC UI
    public TargetRing targetRing;

    private const string CAMERA_NAME = "Main Camera";
    private const string SPAWNUNITBUTTON_NAME = "SpawnUnitButton";
    private const string GOLDAMOUNTTEXT_NAME = "GoldAmountText";
    private const string PLAYER_NAME = "Player";
    private const float WAIT_TIME = 1f;

    private List<IObserver> observers;
    private Unit unitCurrentlyDisplayed;
    private City cityCurrentlyDisplayed;
    private Vector3 oldMousePos;
    private Vector3 menuSpawnPos;

    // **          //
    // * METHODS * //
    //          ** //

    public void NotifyAll(Invocation invoke, params object[] data)
    {
        foreach (IObserver o in observers)
        {
            o.OnNotify(this, invoke, data);
        }
    }

    /// <summary>
    /// Toggles whether or not the pause menu is visible.
    /// </summary>
    public void TogglePauseMenu()
    {
        pauseMenu.enabled = !(pauseMenu.enabled);
        pauseMenu.transform.SetAsLastSibling();
    }

    /// <summary>
    /// Toggles whether or not the pause text is visible.
    /// </summary>
    public void TogglePauseText()
    {
        pauseText.enabled = !(pauseText.enabled);
    }

    /// <summary>
    /// Sets the unit to spawn based on the value of the dropdown menu,
    /// and communicates that to Player.
    /// </summary>
    public void SetUnitToSpawn()
    {
        string toSpawn;
        switch (unitSelect.value)
        {
            case 0:
                toSpawn = Twirl.IDENTITY;
                break;
            default:
                toSpawn = Boomy.IDENTITY;
                break;
        }

        Toolbox.PLAYER.SetUnitToSpawn(toSpawn);

    }

    /// <summary>
    /// Communicates that Player pressed the SpawnUnit button in a CityMenu.
    /// </summary>
    public void SpawnUnit()
    {
        Toolbox.PLAYER.SetCityToSpawnAt(cityCurrentlyDisplayed);
        Toolbox.PLAYER.SpawnUnit();
    }

    /// <summary>
    /// Brings up the unit menu and displays unit's info. Handles any display
    /// info that won't require dynamic updating. Buttons will be disabled or
    /// enabled depending on whether or not the player owns that unit.
    /// </summary>
    /// <param name="unit">The unit whose info is to be displayed.</param>
    public void DisplayUnitInfo(MobileUnit unit)
    {
        // Only allow one highlighting ring
        if (unitCurrentlyDisplayed != null && unitCurrentlyDisplayed != unit)
            unitCurrentlyDisplayed.RemoveHighlight();

        unitCurrentlyDisplayed = unit;
        //float damage = unitCurrentlyDisplayed.Damage;
        //float range = unitCurrentlyDisplayed.Range;
        //int cost = unitCurrentlyDisplayed.Cost;

        // Set position to wherever menus are supposed to appear
        unitMenu.transform.position = menuSpawnPos;

        // Handle unit name input field
        unitMenuName.enabled = unit.Team == Toolbox.PLAYER.Team;
        unitMenuName.placeholder.GetComponent<Text>().text = unit.Name;

        // Handle health slider
        unitMenuHealth.maxValue = unit.MaxHealth;
        unitMenuHealth.value = unit.Health;

        // Once processing is finished, bring to front and enable display
        unitMenu.transform.SetAsLastSibling();
        unitMenu.enabled = true;
    }

    /// <summary>
    /// Displays the city menu. Handles any display info that won't require 
    /// dynamic updating. Buttons will be disabled or enabled depending on 
    /// whether or not the player owns that unit. 
    /// </summary>
    /// <param name="city">The city to display.</param>
    public void DisplayCityInfo(City city)
    {
        // Only allow one highlighting ring
        if (cityCurrentlyDisplayed != null && cityCurrentlyDisplayed != city)
            cityCurrentlyDisplayed.RemoveHighlight();

        cityCurrentlyDisplayed = city;

        // Set position to wherever menus are supposed to appear
        cityMenu.transform.position = menuSpawnPos;

        // Handle city name input field
        cityMenuName.enabled = city.Team == Toolbox.PLAYER.Team;
        cityMenuName.placeholder.GetComponent<Text>().text = city.Name;

        // Handle spawn button
        cityMenuSpawn.enabled = city.Team == Toolbox.PLAYER.Team;

        // Handle sliders
        cityMenuHealth.maxValue = City.MAX_HEALTH;
        cityMenuHealth.value = city.Health;
        cityMenuIncome.maxValue = City.MAX_INCOME_LEVEL;
        cityMenuIncome.value = city.IncomeLevel;

        // Once processing is finished, bring to front and enable display
        cityMenu.transform.SetAsLastSibling();
        cityMenu.enabled = true;
    }

    /// <summary>
    /// Updates the unit menu based on the dynamic status of the unit, if a 
    /// unit is being displayed.
    /// </summary>
    public void UpdateUnitMenu()
    {
        if (!unitMenu.enabled) { return; }

        // Handle health slider
        unitMenuHealth.value = unitCurrentlyDisplayed.Health;
    }

    /// <summary>
    /// Animates the "start round" text of a game.
    /// </summary>
    /// <param name="waitTime">The amount of time to wait before playing the
    /// animation, in seconds.</param>
    public IEnumerator AnimateText(string text)
    {
        // Don't animate if we're already animating something
        if (message.enabled) { yield break; }

        const int FRAMES_TO_LINGER = 60;
        const float MOVE_DISTANCE_SMALL = 30f;
        const float MIN_DISTANCE_SQR = 30000f;
        Color textColor = new Color(1f, 1f, 1f, 0f); // white, but invisible
        Vector3 textPosition = message.transform.position;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        float MOVE_DISTANCE_LARGE = Screen.width / 2;
        textPosition.x = 0;

        yield return null;

        message.text = text;
        message.color = textColor;
        message.transform.position = textPosition;
        message.enabled = true;

        // Until the text is near the center of the screen, move it to the 
        // right and raise the alpha
        while ((message.transform.position - screenCenter).sqrMagnitude > MIN_DISTANCE_SQR)
        {
            textColor.a += 4.5f * Time.deltaTime;
            textPosition.x += MOVE_DISTANCE_LARGE * Time.deltaTime;
            message.color = textColor;
            message.transform.position = textPosition;
            yield return null;
        }

        // Let it linger for FRAMES_TO_LINGER frames
        for (int x = 0; x < FRAMES_TO_LINGER; x++)
        {
            textPosition.x += MOVE_DISTANCE_SMALL * Time.deltaTime;
            message.transform.position = textPosition;
            yield return null;
        }

        // Until text is offscreen, move to the right and fade out
        while (message.transform.position.x < Screen.width * 1.5)
        {
            textColor.a -= 4.5f * Time.deltaTime;
            textPosition.x += MOVE_DISTANCE_LARGE * Time.deltaTime;
            message.color = textColor;
            message.transform.position = textPosition;
            yield return null;
        }

        message.enabled = false;
        NotifyAll(Invocation.ANIMATION_FINISHED);
    }

    // Initialize only once
    private void Awake()
    {
        // Set UI handlers
        // Handlers for changing a dropdown value
        unitSelect.onValueChanged.AddListener(delegate { SetUnitToSpawn(); });
        // Handlers for finishing changing a name field
        unitMenuName.onEndEdit.AddListener(delegate { UpdateUnitName(); });
        unitMenuName.onEndEdit.AddListener(delegate { UpdateCityName(); });
        // Handlers for pressing a button on a menu
        cityMenuSpawn.onClick.AddListener(delegate { SpawnUnit(); });
        resetButton.onClick.AddListener(delegate { ResetButtonPressed(); });
        exitButton.onClick.AddListener(delegate { ExitButtonPressed(); });
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
        targetRing = Instantiate(targetRing);

        // Handle private fields
        menuSpawnPos = unitMenu.transform.position;

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
    /// Exits the game.
    /// </summary>
    /// TODO allow exit back to main menu.
    private void ExitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Updates the city menu based on the dynamic status of the city, if a
    /// city is being displayed.
    /// </summary>
    private void UpdateCityMenu()
    {
        if (!cityMenu.enabled) { return; }

        // Handle sliders
        cityMenuHealth.value = cityCurrentlyDisplayed.Health;
        cityMenuIncome.value = cityCurrentlyDisplayed.IncomeLevel;
    }

    /// <summary>
    /// Updates the currently displayed unit with a custom name.
    /// </summary>
    public void UpdateUnitName()
    {
        unitCurrentlyDisplayed.CustomName = unitMenuName.text;
    }

    /// <summary>
    /// Updates the currently displayed city with a custom name.
    /// </summary>
    public void UpdateCityName()
    {
        cityCurrentlyDisplayed.CustomName = cityMenuName.text;
    }

    /// <summary>
    /// Displays the target ring at the current mouse position.
    /// </summary>
    /// <param name="terrain">The GameObject currently serving as the ground.</param>
    public void DisplayTargetRing(RTS_Terrain terrain)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrain.ignoreAllButTerrain))
        {
            // TODO prevent target ring from sinking into the ground
            targetRing.transform.position = new Vector3(hit.point.x, hit.point.y + 3f, hit.point.z);
            targetRing.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the target ring.
    /// </summary>
    public void HideTargetRing()
    {
        targetRing.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hides all currently displayed menus.
    /// </summary>
    public void CloseAll()
    {
        targetRing.gameObject.SetActive(false);
        pauseText.enabled = false;
        pauseMenu.enabled = false;
        unitMenu.enabled = false;
        cityMenu.enabled = false;
        message.enabled = false;
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
        int gold = Toolbox.PLAYER.Gold;
        string goldText = gold.ToString();
        currentGoldAmount.text = goldText;
    }

    /// <summary>
    /// Updates the number of units a Player has in the overlay.
    /// </summary>
    private void UpdateUnitAmountText()
    {
        int units = Toolbox.PLAYER.Team.mobiles.Count;
        string unitText = units.ToString();
        currentUnitAmount.text = unitText;
    }
}

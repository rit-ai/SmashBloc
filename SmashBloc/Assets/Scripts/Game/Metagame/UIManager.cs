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
    public GameObject pauseText;
    // MAIN MENU
    public GameObject pauseMenu;
    public GameObject mainPausePanel;
    public Button restartButton;
    public Button optionsButton;
    public Button exitButton;
    // OPTIONS MENU
    public GameObject optionsPausePanel;
    public Toggle enableDevToggle;
    // DEV MENU
    public GameObject devMenu;
    // HEADER
    public Dropdown unitSelectDropdown;
    public Text currentGoldText;
    public Text currentUnitsText;
    public Button devMenuButton;
    // STARTING AND ENDING GAME
    public Text startingMessageText;
    // UNIT MENU
    public Canvas unitMenu;
    public InputField unitMenuNameInput;
    public Slider unitMenuHealthSlider;
    // CITY MENU
    public Canvas cityMenu;
    public InputField cityMenuNameInput;
    public Slider cityMenuHealthSlider;
    public Slider cityMenuIncomeSlider;
    public Button cityMenuSpawnButton;
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
    /// Toggles whether or not the pause menu is visible. And will toggle back
    /// to a root menu, if in sub-menu
    /// </summary>
    public void ToggleMenu()
    {
        if(optionsPausePanel.gameObject.activeSelf)
        {
            ToggleOptionsMenu();
        }
        else
        {
            pauseMenu.gameObject.SetActive(!(pauseMenu.gameObject.activeSelf));
            pauseMenu.transform.SetAsLastSibling();
        }
        
    }

    /// <summary>
    /// Toggles whether or not the buttons related to the options menu are
    /// visible. Will also notify observers of current menu state
    /// </summary>
    public void ToggleOptionsMenu()
    {
        mainPausePanel.gameObject.SetActive(!mainPausePanel.gameObject.activeSelf);
        optionsPausePanel.gameObject.SetActive(!optionsPausePanel.gameObject.activeSelf);
        // Notifies all observers of the current menu state
        if (!optionsPausePanel.gameObject.activeSelf)
        {
            NotifyAll(Invocation.IN_MAINMENU);
        }
        else
        {
            NotifyAll(Invocation.IN_SUBMENU);
        }
    }

    /// <summary>
    /// Toggles whether or not the pause text is visible. This is called
    /// because the UI manager holds references to UI elements in the scene
    /// and GameManager does not. This is called by UIObserver.
    /// </summary>
    public void EnablePauseText()
    {
        // Do not enable the pauseText if the pauseMenu is brought up
        pauseText.gameObject.SetActive(true);
    }

    public void DisablePauseText()
    {
        pauseText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the unit to spawn based on the value of the dropdown menu,
    /// and communicates that to Player.
    /// </summary>
    public void SetUnitToSpawn()
    {
        string toSpawn;
        switch (unitSelectDropdown.value)
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
        unitMenuNameInput.enabled = unit.Team == Toolbox.PLAYER.Team;
        unitMenuNameInput.placeholder.GetComponent<Text>().text = unit.Name;

        // Handle health slider
        unitMenuHealthSlider.maxValue = unit.MaxHealth;
        unitMenuHealthSlider.value = unit.Health;

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
        cityMenuNameInput.enabled = city.Team == Toolbox.PLAYER.Team;
        cityMenuNameInput.placeholder.GetComponent<Text>().text = city.Name;

        // Handle spawn button
        cityMenuSpawnButton.enabled = city.Team == Toolbox.PLAYER.Team;

        // Handle sliders
        cityMenuHealthSlider.maxValue = City.MAX_HEALTH;
        cityMenuHealthSlider.value = city.Health;
        cityMenuIncomeSlider.maxValue = City.MAX_INCOME_LEVEL;
        cityMenuIncomeSlider.value = city.IncomeLevel;

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
        unitMenuHealthSlider.value = unitCurrentlyDisplayed.Health;
    }

    /// <summary>
    /// Animates the "start round" text of a game.
    /// </summary>
    /// <param name="waitTime">The amount of time to wait before playing the
    /// animation, in seconds.</param>
    public IEnumerator AnimateText(string text)
    {
        // Don't animate if we're already animating something
        if (startingMessageText.enabled) { yield break; }

        const int FRAMES_TO_LINGER = 60;
        const float MOVE_DISTANCE_SMALL = 30f;
        const float MIN_DISTANCE_SQR = 30000f;
        Color textColor = new Color(1f, 1f, 1f, 0f); // white, but invisible
        Vector3 textPosition = startingMessageText.transform.position;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
        float MOVE_DISTANCE_LARGE = Screen.width / 2;
        textPosition.x = 0;

        yield return null;

        startingMessageText.text = text;
        startingMessageText.color = textColor;
        startingMessageText.transform.position = textPosition;
        startingMessageText.enabled = true;

        // Until the text is near the center of the screen, move it to the 
        // right and raise the alpha
        while ((startingMessageText.transform.position - screenCenter).sqrMagnitude > MIN_DISTANCE_SQR)
        {
            textColor.a += 4.5f * Time.deltaTime;
            textPosition.x += MOVE_DISTANCE_LARGE * Time.deltaTime;
            startingMessageText.color = textColor;
            startingMessageText.transform.position = textPosition;
            yield return null;
        }

        // Let it linger for FRAMES_TO_LINGER frames
        for (int x = 0; x < FRAMES_TO_LINGER; x++)
        {
            textPosition.x += MOVE_DISTANCE_SMALL * Time.deltaTime;
            startingMessageText.transform.position = textPosition;
            yield return null;
        }

        // Until text is offscreen, move to the right and fade out
        while (startingMessageText.transform.position.x < Screen.width * 1.5)
        {
            textColor.a -= 4.5f * Time.deltaTime;
            textPosition.x += MOVE_DISTANCE_LARGE * Time.deltaTime;
            startingMessageText.color = textColor;
            startingMessageText.transform.position = textPosition;
            yield return null;
        }

        startingMessageText.enabled = false;
        NotifyAll(Invocation.ANIMATION_FINISHED);
        yield break;
    }

    // Initialize only once
    private void Awake()
    {
        // Enable/Disable objects
        optionsPausePanel.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("enableDevToggle"))
        {
            enableDevToggle.isOn = (PlayerPrefs.GetInt("enableDevToggle") == 1);
            DevTogglePressed((PlayerPrefs.GetInt("enableDevToggle") == 1));
        }

        // Set UI handlers
        // Handlers for changing a dropdown value
        unitSelectDropdown.onValueChanged.AddListener(delegate { SetUnitToSpawn(); });
        // Handlers for finishing changing a name field
        unitMenuNameInput.onEndEdit.AddListener(delegate { UpdateUnitName(); });
        unitMenuNameInput.onEndEdit.AddListener(delegate { UpdateCityName(); });
        // Handlers for pressing a button on a menu
        cityMenuSpawnButton.onClick.AddListener(delegate { SpawnUnit(); });
        restartButton.onClick.AddListener(delegate { ResetButtonPressed(); });
        exitButton.onClick.AddListener(delegate { ExitButtonPressed(); });
        optionsButton.onClick.AddListener(delegate { OptionsButtonPressed(); });
        devMenuButton.onClick.AddListener(delegate { DevButtonPressed(); });
        cityMenuSpawnButton.onClick.AddListener(delegate { SpawnUnit(); });
        //Handlers for pressing a toggle

        enableDevToggle.onValueChanged.AddListener(DevTogglePressed);
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
        devMenu.transform.SetAsLastSibling();
        devMenu.gameObject.SetActive(!devMenu.gameObject.activeSelf);
    }

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

    private void DevTogglePressed(bool selected)
    {
        devMenuButton.gameObject.SetActive(selected);
    }

    /// <summary>
    /// Updates the city menu based on the dynamic status of the city, if a
    /// city is being displayed.
    /// </summary>
    private void UpdateCityMenu()
    {
        if (!cityMenu.enabled) { return; }

        // Handle sliders
        cityMenuHealthSlider.value = cityCurrentlyDisplayed.Health;
        cityMenuIncomeSlider.value = cityCurrentlyDisplayed.IncomeLevel;
    }

    /// <summary>
    /// Updates the currently displayed unit with a custom name.
    /// </summary>
    public void UpdateUnitName()
    {
        unitCurrentlyDisplayed.CustomName = unitMenuNameInput.text;
    }

    /// <summary>
    /// Updates the currently displayed city with a custom name.
    /// </summary>
    public void UpdateCityName()
    {
        cityCurrentlyDisplayed.CustomName = cityMenuNameInput.text;
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
    /// Hides all currently displayed menus and unpauses the game.
    /// </summary>
    public void CloseAll()
    {
        pauseText.gameObject.SetActive(false);
        targetRing.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        devMenu.gameObject.SetActive(false);
        unitMenu.enabled = false;
        cityMenu.enabled = false;
        startingMessageText.enabled = false;

        Time.timeScale = 1;
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
        devMenuButton.gameObject.SetActive(isOn);
    }

    /// <summary>
    /// Updates the amount of gold a Player has in the overlay.
    /// </summary>
    private void UpdateGoldAmountText()
    {
        int gold = Toolbox.PLAYER.Gold;
        string goldText = gold.ToString();
        currentGoldText.text = goldText;
    }

    /// <summary>
    /// Updates the number of units a Player has in the overlay.
    /// </summary>
    private void UpdateUnitAmountText()
    {
        int units = Toolbox.PLAYER.Team.mobiles.Count;
        string unitText = units.ToString();
        currentUnitsText.text = unitText;
    }
}

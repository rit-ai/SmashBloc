using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * @author Paul Galatic
 * 
 * This game manager manages the game. Pretty simple, eh?
 * 
 * Its responsibilities are pretty complex, so hold onto your coffee. The Game
 * Manager handles the loading of scenes, the starting and ending of rounds, 
 * and also keeps track of all Players, Teams, and game statstics. It receives
 * commands from GameObserver, but can also forward commands to Observers 
 * (particularly UIObserver, as the UI Manager needs to know when rounds start
 * and end).
 * 
 * Everything that involves changing the state of the game as a whole should go
 * in this class. It will be split into smaller files as necessary.
 * **/
public class GameManager : MonoBehaviour, IObservable
{
    // **         //
    // * FIELDS * //
    //         ** //

    [HideInInspector]
    public bool playContinuous; // restart game once it's over
    [HideInInspector]
    public bool pauseOnFinish; // pause game when it's over
    [HideInInspector]
    public bool resetCamera; // reset the camera at the start of a game

    private const string CITY_SPAWN_TAG = "CitySpawn";
    private const float GOLD_INCREMENT_RATE = 0.1f; // higher is slower
    private const int MAX_MONEY = 999; // richness ceiling
    private const int NUM_AI_PLAYERS = 1;

    private List<IObserver> observers;
    private List<Team> teams;
    private List<Player> players;
    private CameraController cameraController;
    private GameObject[] citySpawnPoints;
    private int activeTeams;
    private bool inSubMenu = false;
    private bool inMenu = false;
    private bool paused = false;

    public void NotifyAll(Invocation invoke, params object[] data)
    {
        foreach (IObserver o in observers)
        {
            o.OnNotify(this, invoke, data);
        }
    }

    /// <summary>
    /// Pauses or unpauses the game, depending on the current timescale.
    /// </summary>
    public void TogglePause()
    {
        // Set to 1 if 0, and 0 if 1.
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        if (Time.timeScale == 0) { NotifyAll(Invocation.PAUSE_TEXT_ENABLE); }
        else { NotifyAll(Invocation.PAUSE_TEXT_DISABLE); }
    }


    /// <summary>
    /// Sets the new destination for the unit, if the unit is of the player's
    /// team. Should only be called via a user's actions.
    /// </summary>
    /// <param name="terrain">The terrain, which was right clicked such to 
    /// invoke this method.</param>
    public void SetNewDestination(List<MobileUnit> selectedUnits, RTS_Terrain terrain)
    {
        if (selectedUnits == null) { return; }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Team playerTeam = Toolbox.PLAYER.Team;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrain.ignoreAllButTerrain))
        {
            // A Thought is used here so that the Player doesn't have a greater
            // degree of control over Mobiles than the AI
            new SendMobilesToLoc(
                // Get all the units selected that the player owns
                selectedUnits.Where(unit => unit.Team == Toolbox.PLAYER.Team).ToList(),
                // Send them to this location
                hit.point
            ).Act();
        }
    }

    /// <summary>
    /// Transfers a city from the control of one Team to another, and destroys 
    /// a team if they have no remaining cities.
    /// </summary>
    /// <param name="city">The city to be transferred.</param>
    /// <param name="newTeam">The team the city will be transferred to.</param>
    public void TransferCity(City city, Team newTeam)
    {
        Team loser = city.Team;
        city.Team.cities.Remove(city);
        newTeam.cities.Add(city);

        // Don't change the city's team until this point
        city.Team = newTeam;

        if (loser.IsActive && loser.cities.Count == 0)
        {
            loser.Deactivate();
            activeTeams--;
        }
    }

    /// <summary>
    /// Resets the game to its original state.
    /// </summary>
    public void ResetGame()
    {

        StopAllCoroutines();
        NotifyAll(Invocation.CLOSE_ALL);

        foreach (Team t in teams)
        {
            t.Deactivate();
        }

        // REACTIVATION

        DistributeCities();

        // Activate all the teams
        foreach (Team t in teams)
        {
            t.Activate();
        }

        activeTeams = teams.Count;

        // Set main camera to be behind the first city
        // TODO make this more flexible
        if (resetCamera) { cameraController.CenterCameraBehindPosition(teams[0].cities[0].transform.position); }

        StartCoroutine(GameLoop());

    }

    /// <summary>
    /// Starts the game loop and takes care of any low-priority initialization.
    /// </summary>
    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        citySpawnPoints = GameObject.FindGameObjectsWithTag(CITY_SPAWN_TAG);

        Debug.Assert(cameraController != null);
        Debug.Assert(citySpawnPoints != null && citySpawnPoints.Length > 1);

        teams = Toolbox.GameSetup.Teams;
        players = Toolbox.GameSetup.Players;
        activeTeams = teams.Count;

        observers = new List<IObserver>
        {
            Toolbox.UIObserver
        };

        DistributeCities();

        // Activate all the teams
        foreach (Team t in teams)
        {
            t.Activate();
        }

        // Set main camera to be behind a city, preferrably the player's
        // Somewhat inexact, TODO make sure it finds the first city every time
        cameraController.CenterCameraBehindPosition(teams[0].cities[0].transform.position);

        StartCoroutine(GameLoop());

    }

    /// <summary>
    /// Simple game loop that goes through the three phases--Starting, Playing,
    /// and Ending--then repeats.
    /// </summary>
    private IEnumerator GameLoop()
    {
        if (Time.timeScale == 0) { TogglePause(); }

        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());
    }

    /// <summary>
    /// Starts the round.
    /// 
    /// FIXME For some reason, Game Manager isn't giving Toolbox enough time to
    /// find the UI Observer before starting the game.
    /// </summary>
    private IEnumerator RoundStarting()
    {
        yield return new WaitForSeconds(1); // FIXME

        NotifyAll(Invocation.GAME_STARTING);

        yield return new WaitForSeconds(1);

        // Start IEnumerators
        StartCoroutine(IncrementGold());
    }

    /// <summary>
    /// This loop runs until there is only one team left.
    /// </summary>
    private IEnumerator RoundPlaying()
    {
        yield return new WaitUntil(() => activeTeams == 1);
    }

    /// <summary>
    /// Finishes the round.
    /// </summary>
    private IEnumerator RoundEnding()
    {
        // waitingOnAnimation = true; // TODO wait for ending animation
        NotifyAll(Invocation.GAME_ENDING);
        yield return new WaitForSeconds(3f);
        if (playContinuous)
        {
            NotifyAll(Invocation.RESET_GAME);
            ResetGame();
            yield break;
        }

        if (pauseOnFinish)
        {
            TogglePause();
            NotifyAll(Invocation.PAUSE_AND_LOCK);
        }
    }

    /// <summary>
    /// Builds cities at the city spawn points and gives each Player a City.
    /// 
    /// The number of cities to instantiate is capped both by the number of
    /// places to spawn them as well as the total number of players in the 
    /// game. 
    /// </summary>
    private void DistributeCities()
    {
        // Every player should have at least one city, and we need places to 
        // put them.
        Debug.Assert(citySpawnPoints.Length >= NUM_AI_PLAYERS + 1);

        City city;
        int currTeam = 0;
        int currCity = 0;

        // Don't distrubute cities if there aren't any teams
        if (teams.Count < 1) { Debug.Log("Did you forget to set the number of teams?");  return; }

        // Run until we run out of spawn points or we run out of players
        while (currCity < citySpawnPoints.Length && currCity < NUM_AI_PLAYERS + 1)
        {
            // Grab a city from the city pool and initialize it
            city = Toolbox.CityPool.Rent();
            city.Team = teams[currTeam];
            teams[currTeam].cities.Add(city);
            city.transform.position = citySpawnPoints[currCity].transform.position;
            city.gameObject.SetActive(true);
            city.Activate();
            // Increment and wrap around the list of teams
            currTeam = ++currTeam % teams.Count;
            // Increment the city spawn counter
            currCity++;
        }
    }

    /// <summary>
    /// Updates the current gold amount, of all players reflecting income from
    /// cities.
    /// </summary>
    private IEnumerator IncrementGold()
    {
        while (true)
        {

            foreach (Player p in players)
            {
                foreach (City c in p.Team.cities)
                {
                    p.Gold += c.IncomeLevel;
                    if (p.Gold > MAX_MONEY)
                    {
                        p.Gold = MAX_MONEY;
                    }
                }
            }

            yield return new WaitForSeconds(GOLD_INCREMENT_RATE);

        }
    }

    /// <summary>
    /// Getter/Setter for inMenu
    /// </summary>
    public bool InMenu
    {
        get { return inMenu; }
        set { inMenu = value; }
    }

    /// <summary>
    /// Getter/Setter for inSubMenu
    /// </summary>
    public bool InSubMenu
    {
        get { return inSubMenu; }
        set { inSubMenu = value; }
    }

    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }
}

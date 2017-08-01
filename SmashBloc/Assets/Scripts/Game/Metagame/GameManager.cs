using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
 * in this class.
 * **/
public class GameManager : MonoBehaviour, IObservable {

    // Public constants

    [HideInInspector]
    public bool playContinuous; // restart game once it's over

    private const string CITY_SPAWN_TAG = "CitySpawn";
    private const float GOLD_INCREMENT_RATE = 0.1f; // higher is slower
    private const int MAX_MONEY = 999; // richness ceiling
    private const int NUM_AI_PLAYERS = 1;

    private CameraController m_CameraController;
    private GameObject[] citySpawnPoints;

    private List<Team> teams;
    private List<Player> players;
    private List<IObserver> observers;

    private int activeTeams;

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
    }

    /// <summary>
    /// Sets the new destination for the unit, if the unit is of the player's
    /// team. New destinations take the form of MoveCommands, which means that 
    /// the unit will deviate from its destination based on its 
    /// destDeviationRadius value.
    /// </summary>
    /// <param name="terrain">The terrain, which was right clicked such to 
    /// invoke this method.</param>
    public void SetNewDestination(HashSet<MobileUnit> selectedUnits, RTS_Terrain terrain)
    {
        if (selectedUnits == null) { return; }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Team playerTeam = Toolbox.PLAYER.Team;
        if (Physics.Raycast(ray, out hit, terrain.ignoreAllButTerrain))
        {
            // Set the destination of all the units
            MoveCommand move = new MoveCommand(hit.point);
            foreach (MobileUnit u in selectedUnits)
            {
                if (u.Team == playerTeam)
                {
                    move.Body = u;
                    move.Execute();
                }
            }
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
        city.Team.cities.Remove(city);
        newTeam.cities.Add(city);

        // Don't chance the city's team until the end
        city.Team = newTeam;

        // Have all the team's cities been eliminated?
        foreach (Team t in teams)
        {
            if (t.IsActive && t.cities.Count == 0)
            {
                t.Deactivate();
                activeTeams--;
            }
        }
    }

    /// <summary>
    /// Resets the game to its original state.
    /// </summary>
    public void ResetGame()
    {
        // DEACTIVATION

        // Deactivate all teams
        foreach (Team t in teams)
        {
            t.Deactivate();
            t.Activate();
        }

        StopAllCoroutines();

        NotifyAll(Invocation.CLOSE_ALL);

        // REACTIVATION

        DistributeCities();

        // Activate all the teams
        foreach (Team t in teams)
        {
            t.Activate();
        }

        activeTeams = teams.Count;

        // Set main camera to be behind the first city
        // TODO make this more exact
        m_CameraController.CenterCameraBehindPosition(teams[0].cities[0].transform.position);

        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// Starts the game loop and takes care of any low-priority initialization.
    /// </summary>
    private void Start()
    {
        m_CameraController = Camera.main.GetComponent<CameraController>();
        citySpawnPoints = GameObject.FindGameObjectsWithTag(CITY_SPAWN_TAG);

        Debug.Assert(m_CameraController != null);
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
        m_CameraController.CenterCameraBehindPosition(teams[0].cities[0].transform.position);

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
        // Stop IEnumerators
        StopCoroutine(IncrementGold());

        // waitingOnAnimation = true; // TODO wait for ending animation
        NotifyAll(Invocation.GAME_ENDING);
        yield return new WaitForSeconds(3f);
        if (playContinuous)
        {
            NotifyAll(Invocation.RESET_GAME);
            ResetGame();
            yield break;
        }
        TogglePause();
        NotifyAll(Invocation.PAUSE_AND_LOCK);
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

    public enum GameState { STARTING, PLAYING, ENDING }
}

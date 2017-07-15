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
    // The first created player, which will always be the main player.
    public static Player PLAYER;

    private const string CITY_SPAWN_TAG = "CitySpawn";
    private const float GOLD_INCREMENT_RATE = 0.1f; // higher is slower
    private const int MAX_MONEY = 999; // richness ceiling
    private const int NUM_AI_PLAYERS = 1;

    private CameraController m_CameraController;
    private RTS_Terrain m_Terrain;
    private GameObject[] citySpawnPoints;

    private List<Team> teams;
    private List<Player> players;
    private List<IObserver> observers;

    private bool waitingOnAnimation = false;

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
    /// team.
    /// </summary>
    /// <param name="terrain">The terrain, which was right clicked such to 
    /// invoke this method.</param>
    public void SetNewDestination(HashSet<MobileUnit> selectedUnits, RTS_Terrain terrain)
    {
        if (selectedUnits == null) { return; }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Team playerTeam = PLAYER.Team;
        if (Physics.Raycast(ray, out hit, terrain.ignoreAllButTerrain))
        {
            // Set the destination of all the units
            foreach (MobileUnit u in selectedUnits)
            {
                if (u.Team == playerTeam)
                    u.Destination = hit.point;
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
            if (t.cities.Count == 0)
            {
                t.DestroyTeam();
            }
        }

        // This is a little redundant, but won't matter unless we want 1000+ 
        // teams.
        teams.RemoveAll(t => t.cities.Count == 0);
    }

    /// <summary>
    /// Resets the game to its original state.
    /// </summary>
    public void ResetGame()
    {
        // Destroy all teams
        foreach (Team t in teams)
        {
            t.DestroyTeam();
        }

        DistributeCities();

        foreach (Player p in players)
        {
            p.Start();
        }

        // Set main camera to be behind the player's first city
        m_CameraController.CenterCameraBehindPosition(PLAYER.Team.cities[0].transform.position, m_Terrain.transform.position);

        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// EXTREMELY IMPORTANT INITAILIZATION METHOD.
    /// 
    /// This is one of the first initialization methods to occur in the game. 
    /// Since Game Manager forwards its state to many other classes, ensuring 
    /// said state is valid and accurate is very important to avoid crashes.
    /// 
    /// When editing this method, take care to validate you additions with 
    /// Debug.Assert().
    /// </summary>
    private void Awake()
    {
        m_CameraController = Camera.main.GetComponent<CameraController>();

        m_Terrain = GameObject.FindGameObjectWithTag(RTS_Terrain.TERRAIN_TAG).GetComponent<RTS_Terrain>();
        citySpawnPoints = GameObject.FindGameObjectsWithTag(CITY_SPAWN_TAG);

        Debug.Assert(m_CameraController != null);
        Debug.Assert(m_Terrain != null);
        Debug.Assert(citySpawnPoints != null && citySpawnPoints.Length > 1);

        observers = new List<IObserver>
        {
            gameObject.AddComponent<UIObserver>()
        };

        teams = new List<Team>
        {
            new Team("Dylante", Color.cyan),
            new Team("AI Team", Color.red)
        };

        players = new List<Player>
        {
            Player.MakePlayer(false, teams[0]),
            Player.MakePlayer(true, teams[1])
        };

        PLAYER = players[0];
    }

    /// <summary>
    /// Starts the game loop and takes care of any low-priority initialization.
    /// </summary>
    private void Start()
    {
        DistributeCities();

        // Set main camera to be behind the player's first city
        m_CameraController.CenterCameraBehindPosition(teams[0].cities[0].transform.position, m_Terrain.transform.position);

        StartCoroutine(GameLoop());

    }

    /// <summary>
    /// Simple game loop that goes through the three phases--Starting, Playing,
    /// and Ending--then repeats.
    /// </summary>
    private IEnumerator GameLoop()
    {
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
        yield return new WaitForSeconds(1); // FIXMe

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
        yield return new WaitUntil(() => teams.Count == 1);
    }

    /// <summary>
    /// Finishes the round.
    /// </summary>
    private IEnumerator RoundEnding()
    {
        // Stop IEnumerators
        StopCoroutine(IncrementGold());

        waitingOnAnimation = true; // wait for ending animation
        NotifyAll(Invocation.GAME_ENDING);
        yield return new WaitUntil(() => waitingOnAnimation == false);
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

        City curr;
        for (int x = 0; ((x < citySpawnPoints.Length) && (x < NUM_AI_PLAYERS + 1)); x++)
        {
            curr = Toolbox.Pool.RetrieveCity(teams[x], citySpawnPoints[x].transform.position);
            teams[x].cities.Add(curr);
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Sets up a game, giving the designers more flexibility when setting up 
 * scenes. This class is designed to be attached to Toolbox, as it provides 
 * state to GameManager, which initializes the game. This class is merely 
 * designed to coordinate the decisions of the designers and initialize some
 * basic state.
 * **/
public class GameSetup : MonoBehaviour
{
    // **         
    // * FIELDS * //
    //         ** //

    [Tooltip("Will this game have any human player?")]
    public bool hasPlayer;
    [Tooltip("Minimum: 1; Maximum: Number of CitySpawnPoints")]
    public int numberOfTeams;

    // 'Standard' colors to make teams easily distinguishible
    private List<Color> DEFAULT_COLORS = new List<Color>
        {
            Color.cyan,
            Color.red,
            Color.yellow,
            Color.green
        };
    private List<Team> teams;
    private List<Player> players;
    [SerializeField]
    private List<string> teamNames;
    private bool locked; // Are the controls locked?

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Picks A random name from the names list for a team
    /// </summary>
    private string chooseName()
    {
        //check if there are any names left
        if(teamNames.Count > 0)
        {
            //select the name from a random point in the names array
            string selectedName = teamNames[(int)(Random.value * teamNames.Count)];
            //remove the selected name from the list
            teamNames.Remove(selectedName);
            Debug.Log("Selected name: " + selectedName);
            return selectedName;
        }
        return "No Names Left";
    }

    /// <summary>
    /// This should be called by Toolbox at the start of the level.
    /// </summary>
    public void Init()
    {
        // Make sure that there is at least one team (for stability)
        players = new List<Player>();
        teams = new List<Team>
        {
            new Team(chooseName(), DEFAULT_COLORS[0])
        };

        if (hasPlayer)
        {
            players.Add(Player.MakePlayer(false, teams[0]));
        }
        else
        {
            players.Add(Player.MakePlayer(true, teams[0]));
        }

        // Controls are locked if hasPlayer = false, true otherwise
        locked = !hasPlayer;

        // The rest are AI teams
        for (int x = 1; x < numberOfTeams; x++)
        {
            teams.Add(new Team(chooseName(), DEFAULT_COLORS[x]));
            players.Add(Player.MakePlayer(true, teams[x]));
        }

        // Add all the enemy info
        for (int x = 0; x < numberOfTeams; x++)
        {
            List<Team> enemies = new List<Team>();
            for (int y = 0; y < numberOfTeams; y++)
            {
                if (y == x) { continue; } // skip the current team
                enemies.Add(teams[y]);
            }
            teams[x].enemies = enemies;
        }
    }

    public List<Team> Teams
    {
        get { return teams; }
    }

    public List<Player> Players
    {
        get { return players; }
    }

    public bool Locked
    {
        get { return locked; }
    }
}

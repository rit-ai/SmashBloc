using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Sets up a game, giving the designers more flexibility when setting up 
 * scenes. Designed to be attached to Toolbox, and provides state to 
 * GameManager, which initializes the game. This class is merely designed to 
 * establish basic state like Player and Team.
 * **/
public class GameSetup : MonoBehaviour {

    [Tooltip("Will this game have any human player?")]
    public bool hasPlayer;
    [Tooltip("Minimum: 1; Maximum: Number of CitySpawnPoints")]
    public int numberOfTeams;

    private List<Team> teams;
    private List<Player> players;
    private bool locked; // Are the controls locked?

    /// <summary>
    /// This should be called by Toolbox at the start of the level.
    /// </summary>
    public void Init()
    {
        // Make sure that there is at least one team (for stability)
        players = new List<Player>();
        teams = new List<Team>
        {
            new Team("TEAM ONE", Random.ColorHSV(0f, 1f))
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
            teams.Add(new Team(x.ToString(), Random.ColorHSV(0f, 1f)));
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

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

    public int NumberOfTeams;

    private List<Team> teams;
    private List<Player> players;

    private void Awake()
    {
        players = new List<Player>();
        teams = new List<Team>
        {
            // Add one at the start for the player specifically
            new Team("Player", Random.ColorHSV(0f, 1f, 1f, 1f))
        };
        players.Add(Player.MakePlayer(false, teams[0]));

        // The rest are AI teams
        for (int x = 1; x < NumberOfTeams; x++)
        {
            teams.Add(new Team(x.ToString(), Random.ColorHSV(0f, 1f)));
            players.Add(Player.MakePlayer(true, teams[x]));
        }

        // Add all the enemy info
        for (int x = 0; x < NumberOfTeams; x++)
        {
            List<Team> enemies = new List<Team>();
            for (int y = 0; y < NumberOfTeams; y++)
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Suite to enable and disable different information about the game state.
 * **/
public class Debuggy : MonoBehaviour {

    public readonly bool twirls;
    public bool lasers;

    // add more debug options here!

    // These Statics are used by the rest of the program, initialized upon game load.
    public static bool Twirls = false;
    public static bool Lasers = false;

    private static Team debugTeam = new Team("WHOOPS PLEASE EDIT", Color.white);

    /// <summary>
    /// Sets up a Twirl for the purposes of debugging, NOT for actual play.
    /// </summary>
    public static Twirl DebugSetupTwirl(Twirl t)
    {
        t.gameObject.AddComponent<TwirlPhysics>();
        t.Team = debugTeam;
        t.Brain = t.gameObject.AddComponent<MobileAI_Basic>();
        t.Brain.Body = t;
        t.Build();
        t.Activate();

        return t;
    }

    public void Awake()
    {
        Twirls = twirls;
        Lasers = lasers;
    }
}

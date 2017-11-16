using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Suite to enable and disable different information about the game state.
 * **/
public class Debuggy : MonoBehaviour {

    public bool Twirls;
    public bool Lasers;

    // add more debug options here!

    private Team debugTeam = new Team("WHOOPS PLEASE EDIT", Color.white);

    /// <summary>
    /// Sets up a Twirl for the purposes of debugging, NOT for actual play.
    /// </summary>
    public Twirl DebugSetupTwirl(Twirl t)
    {
        t.gameObject.AddComponent<TwirlPhysics>();
        t.Team = debugTeam;
        Debug.Log("yes");
        t.AI = t.gameObject.AddComponent<MobileAI_Basic>();
        t.AI.Body = t;
        t.Build();
        t.Activate();

        return t;
    }
}

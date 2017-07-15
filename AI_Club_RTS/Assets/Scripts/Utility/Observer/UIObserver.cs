using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This class listens to other classes to determine which menus should be
 * displayed.
 * **/
public class UIObserver : MonoBehaviour, IObserver {
    // Private constant fields
    // We keep a reference to the UI Manager to tell it what we want it to show
    private static UIManager manager;
    private static Team PLAYER_TEAM;
    private static string START_GAME_TEXT = "BEGIN!";
    private static string END_GAME_TEXT = "FINISH!";

    /// <summary>
    /// Find the UI Manager and store a reference to it.
    /// </summary>
    private void Start()
    {
        manager = Toolbox.UIManager;

        Debug.Assert(manager);

        PLAYER_TEAM = GameManager.PLAYER.Team;
    }

    /// <summary>
    /// Determines which type of menu to raise, depending on the entity 
    /// performing the invocation and the data representing the call.
    /// </summary>
    /// <param name="entity">The entity performing the invocation.</param>
    /// <param name="invoke">The type of invocation.</param>
    /// <param name="data">Optional misc data.</param>
    public void OnNotify(object entity, Invocation invoke, params object[] data)
    {
        bool enabled = false;
        switch (invoke)
        {
            // Plays the starting animation text
            case Invocation.GAME_STARTING:
                StartCoroutine(manager.AnimateText(START_GAME_TEXT));
                break;
            // Plays the ending animation text
            case Invocation.GAME_ENDING:
                StartCoroutine(manager.AnimateText(END_GAME_TEXT));
                break;
            // Opens the pause menu
            case Invocation.PAUSE_AND_LOCK:
                manager.TogglePauseMenu();
                goto case Invocation.TOGGLE_PAUSE;
            // Toggles the pause text
            case Invocation.TOGGLE_PAUSE:
                manager.TogglePauseText();
                break;
            // Display unit info
            case Invocation.UNIT_MENU:
                Debug.Assert(entity is MobileUnit); // don't pass bad objects
                enabled = (((MobileUnit)entity).Team == PLAYER_TEAM);
                manager.DisplayUnitInfo((MobileUnit)entity, enabled);
                break;
            // Display city info
            case Invocation.CITY_MENU:
                Debug.Assert(entity is City); // don't pass bad objects
                enabled = (((City)entity).Team == PLAYER_TEAM);
                manager.DisplayCityInfo((City)entity, enabled);
                break;
            // Moves and renders the target ring
            case Invocation.TARGET_RING:
                Debug.Assert(entity is RTS_Terrain);
                manager.DisplayTargetRing((RTS_Terrain)entity);
                break;
            // Hides all menus and selection elements
            case Invocation.CLOSE_ALL:
                foreach (Unit u in Utils.AllUnits())
                {
                    u.RemoveHighlight();
                }
                foreach (City c in Utils.AllCities())
                {
                    c.RemoveHighlight();
                }
                manager.CloseAll();
                break;
            // Invocation not found? Must be for someone else. Ignore.

        }
    }

}

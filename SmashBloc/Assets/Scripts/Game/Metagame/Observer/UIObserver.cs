using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This class listens to other classes to determine which menus should be
 * displayed, and then relays that information to the UIManager.
 * **/
public class UIObserver : MonoBehaviour, IObserver
{
    // **         //
    // * FIELDS * //
    //         ** //

    private const string START_GAME_TEXT = "BEGIN!";
    private const string END_GAME_TEXT = "FINISH!";
    private static UIManager manager;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Determines which type of menu to raise, depending on the entity 
    /// performing the invocation and the data representing the call.
    /// </summary>
    /// <param name="entity">The entity performing the invocation.</param>
    /// <param name="invoke">The type of invocation.</param>
    /// <param name="data">Optional misc data.</param>
    public void OnNotify(object entity, Invocation invoke, params object[] data)
    {
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
                manager.ToggleMenu();
                break;
                //goto case Invocation.TOGGLE_PAUSE;

            // Toggles the pause text
            case Invocation.TOGGLE_PAUSE:
                manager.TogglePauseText();
                break;

            // Display unit info
            case Invocation.UNIT_MENU:
                Debug.Assert(entity is MobileUnit); // don't pass bad objects
                manager.DisplayUnitInfo((MobileUnit)entity);
                break;

            // Display city info
            case Invocation.CITY_MENU:
                Debug.Assert(entity is City); // don't pass bad objects
                manager.DisplayCityInfo((City)entity);
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

    /// <summary>
    /// Finds and stores a reference to the UIManager.
    /// </summary>
    private void Start()
    {
        manager = Toolbox.UIManager;

        Debug.Assert(manager);
    }

}

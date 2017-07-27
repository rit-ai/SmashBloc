using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft;

/*
 * @author Paul Galatic
 * 
 * Class designed to coordinate the passage of data between GameObjects of 
 * different subtypes. It is also designed to forward messages relevant to the
 * coordination of the game to GameManager.
 * 
 * Domain-limiting is important because, for example, it prevents changes in
 * either the RTS_Terrain class or the Unit class from affecting the other, 
 * since they both communicate through GameObserver instead of directly 
 * referencing one another.
 * **/
public class GameObserver : MonoBehaviour, IObserver {
    // Private fields
    // static because there are multiple GameObservers
    private static HashSet<MobileUnit> selectedUnits;
    private static GameManager manager;

    /// <summary>
    /// Find the Game Manager and store a reference to it.
    /// </summary>
    private void Awake()
    {
        manager = Toolbox.GameManager;
        selectedUnits = new HashSet<MobileUnit>(); // error-proofing
    }

    /// <summary>
    /// Determines the type of action to perform, based on the invocation.
    /// </summary>
    /// <param name="entity">The entity performing the invocation.</param>
    /// <param name="invoke">The type of invocation.</param>
    /// <param name="data">Misc data.</param>
    public void OnNotify(object entity, Invocation invoke, params object[] data)
    {
        switch (invoke)
        {
            // Pause the game
            case Invocation.PAUSE_AND_LOCK:
            case Invocation.TOGGLE_PAUSE:
                manager.TogglePause();
                break;
            // Reset the game
            case Invocation.RESET_GAME:
                manager.ResetGame();
                break;
            case Invocation.IN_SUBMENU:
                manager.InSubMenu = true;
                break;
            case Invocation.IN_MAINMENU:
                manager.InSubMenu = false;
                break;
            // Store selected units (just one)
            case Invocation.ONE_SELECTED:
                Debug.Assert(entity is Unit);
                selectedUnits = new HashSet<MobileUnit> { entity as MobileUnit };
                break;
            // Store selected units (many)
            case Invocation.UNITS_SELECTED:
                Debug.Assert(data != null);
                Debug.Assert(data[0] is HashSet<MobileUnit>);
                selectedUnits = data[0] as HashSet<MobileUnit>;
                Debug.Assert(selectedUnits != null);
                break;
            // Clear stored units
            case Invocation.UNITS_DESELECTED:
                selectedUnits.Clear();
                break;
            // Set new destination based on mouse position over terrain
            case Invocation.DESTINATION_SET:
                Debug.Assert(entity is RTS_Terrain);
                manager.SetNewDestination(selectedUnits, (RTS_Terrain)entity);
                break;
            case Invocation.CITY_CAPTURED:
                Debug.Assert(entity is City);
                Debug.Assert(data != null);
                Debug.Assert(data.Length == 1);
                Debug.Assert(data[0] is Team);
                manager.TransferCity(entity as City, data[0] as Team);
                break;
            // Invocation not found? Must be for someone else. Ignore.
        }
    }

}

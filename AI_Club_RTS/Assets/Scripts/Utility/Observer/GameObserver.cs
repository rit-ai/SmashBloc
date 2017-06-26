using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft;

/*
 * @author Paul Galatic
 * 
 * Class designed to coordinate events that occur within the game while also
 * limiting the domain contained in each Observable. 
 * 
 * An example of this would be having the Terrain communicate to GameObserver
 * that it was right clicked and letting GameObserver handle the rest, rather
 * than having Terrain communicate directly to each selected unit that its
 * destination has changed.
 * **/
public class GameObserver : Observer {

    // Public constant invocations
    public const string UNITS_SELECTED = "UNITS_SELECTED";
    public const string UNITS_DESELECTED = "UNITS_DESELECTED";
    public const string DESTINATION_SET = "DESTINATION_SET";

    // Private fields
    // static because there are multiple GameObservers
    private static HashSet<Unit> selectedUnits; 

    /// <summary>
    /// Determines the type of action to perform, based on the invocation.
    /// </summary>
    /// <param name="entity">The entity performing the invocation.</param>
    /// <param name="invocation">The type of invocation.</param>
    /// <param name="data">Misc data.</param>
    public void OnNotify<T>(Object entity, string invocation, params T[] data)
    {
        switch (invocation)
        {
            // Store units that are selected
            case UNITS_SELECTED:
                Debug.Assert(data != null);
                Debug.Assert(data[0] is HashSet<Unit>);
                selectedUnits = data[0] as HashSet<Unit>;
                Debug.Assert(selectedUnits != null);
                break;
            // Clear stored units
            case UNITS_DESELECTED:
                selectedUnits.Clear();
                break;
            // Set new destination based on mouse position over terrain
            case DESTINATION_SET:
                Debug.Assert(entity is RTS_Terrain);
                SetNewDestination((RTS_Terrain)entity);
                break;
            // Invocation not found? Must be for someone else. Ignore.
        }
    }

    /// <summary>
    /// Sets the new destination for the unit, if the unit is of the player's
    /// team.
    /// </summary>
    /// <param name="terrain">The terrain, which was right clicked such to 
    /// invoke this method.</param>
    private void SetNewDestination(RTS_Terrain terrain)
    {
        if (selectedUnits == null) { return; }
        Camera camera = Camera.main;
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        string playerTeam = Utils.PlayerTeamName;
        if (Physics.Raycast(ray, out hit, terrain.ignoreAllButTerrain))
        {
            // Set the destination of all the units
            foreach (Unit u in selectedUnits)
            {
                if (u.Team == playerTeam)
                    u.SetDestination(hit.point);
            }
        }


    }

}

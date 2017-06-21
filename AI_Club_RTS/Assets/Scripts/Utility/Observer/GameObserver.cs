using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<Unit> selectedUnits;

    public GameObserver()
    {
        selectedUnits = new List<Unit>();
    }

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
                Debug.Assert(data[0] is List<Unit>);
                selectedUnits = data[0] as List<Unit>;
                break;
            // Clear stored units
            case UNITS_DESELECTED:
                selectedUnits.Clear();
                break;
            // Set the destination of all selected units
            case DESTINATION_SET:
                Debug.Assert(data != null);
                Debug.Assert(data[0] is Vector3);
                foreach (Unit u in selectedUnits)
                {
                    u.SetDestination((dynamic)data[0]);
                }
                break;
            // Invocation not found? Must be for someone else. Ignore.
        }
    }

}

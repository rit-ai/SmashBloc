using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This class listens to other classes to determine which menus should be
 * displayed.
 * **/
public class MenuObserver : Observer {

    // Constants that refer to different possible operations of MenuObserver
    public const string INVOKE_UNIT_DATA = "INVOKE_UNIT_DATA";
    public const string SUPPRESS_UNIT_DATA = "SUPPRESS_UNIT_DATA";

    // Private constant fields
    // We keep a reference to the UI Manager to tell it what we want it to show
    private static UI_Manager m_UI_Manager;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="manager">Since we're displaying menus on the overlay, we 
    /// need access to the UI Manager. But, we don't want the UI Manager to 
    /// have to worry about handling menu display logic.</param>
    public MenuObserver(UI_Manager manager)
    {
        m_UI_Manager = manager;
    }

    /// <summary>
    /// Determines which type of menu to raise, depending on the entity 
    /// performing the invocation (if any) and the data representing the 
    /// call.
    /// </summary>
    /// <param name="entity">The entity performing the invocation.</param>
    /// <param name="data">The type of invocation.</param>
    public void OnNotify(Object entity, string data)
    {
        switch (data)
        {
            // Display unit info
            case INVOKE_UNIT_DATA:
                Debug.Assert(entity is Unit);
                m_UI_Manager.DisplayUnitInfo((Unit)entity);
                break;
            // Hide unit info
            case SUPPRESS_UNIT_DATA:
                m_UI_Manager.HideUnitInfo();
                break;
            // Invalid tag received? Must be for someone else. Ignore.
            default:
                break;

        }
    }

}

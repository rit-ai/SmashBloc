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
    public const string INVOKE_CITY_DATA = "INVOKE_CITY_DATA";
    public const string CLOSE_ALL = "CLOSE_ALL";

    // Private constant fields
    // We keep a reference to the UI Manager to tell it what we want it to show
    private static UI_Manager m_UI_Manager = GameObject.FindObjectOfType<UI_Manager>();

    /// <summary>
    /// Determines which type of menu to raise, depending on the entity 
    /// performing the invocation (if any) and the data representing the 
    /// call.
    /// </summary>
    /// <param name="entity">The entity performing the invocation.</param>
    /// <param name="data">The type of invocation.</param>
    public void OnNotify<T>(Object entity, T data)
    {
        if (!(data is string)) { return; }  // irrelevant data, ignore
        switch (data.ToString())            // Convert data to string (it should already be a string)
        {
            // Display unit info
            case INVOKE_UNIT_DATA:
                Debug.Assert(entity is Unit); // don't pass bad objects
                m_UI_Manager.DisplayUnitInfo((Unit)entity);
                break;
            // Hides all menus and selection elements
            case INVOKE_CITY_DATA:
                Debug.Assert(entity is City); // don't pass bad objects
                m_UI_Manager.DisplayCityInfo((City)entity);
                break;
            case CLOSE_ALL:
                foreach (Unit u in Utils.AllUnits())
                {
                    u.RemoveHighlight();
                }
                m_UI_Manager.CloseAll();
                break;
                // Invalid tag received? Must be for someone else. Ignore.

        }
    }

}

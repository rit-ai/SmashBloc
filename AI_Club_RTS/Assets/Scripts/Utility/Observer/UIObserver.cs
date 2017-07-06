using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This class listens to other classes to determine which menus should be
 * displayed.
 * **/
public class UIObserver : IObserver {
    // Private constant fields
    // We keep a reference to the UI Manager to tell it what we want it to show
    private static UI_Manager m_UI_Manager = Object.FindObjectOfType<UI_Manager>();
    private static Team PLAYER_TEAM = Utils.PlayerOne.Team;
    
    

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
            // Display unit info
            case Invocation.UNIT_MENU:
                Debug.Assert(entity is Unit); // don't pass bad objects
                enabled = (((Unit)entity).Team == PLAYER_TEAM);
                m_UI_Manager.DisplayUnitInfo((Unit)entity, enabled);
                break;
            // Display city info
            case Invocation.CITY_MENU:
                Debug.Assert(entity is City); // don't pass bad objects
                enabled = (((City)entity).Team == PLAYER_TEAM);
                m_UI_Manager.DisplayCityInfo((City)entity, enabled);
                break;
            case Invocation.TARGET_RING:
                Debug.Assert(entity is RTS_Terrain);
                m_UI_Manager.DisplayTargetRing((RTS_Terrain)entity);
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
                m_UI_Manager.CloseAll();
                break;
            // Invocation not found? Must be for someone else. Ignore.

        }
    }

}

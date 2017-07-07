using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This file contains the Invocation enum, which is designed specifically for 
 * use with Observers. When an event occurs, an Observable object sends an 
 * Invocation, along with some data, to all of its Observers, who then perform
 * behavior based on whether or not they are programmed to recognize the 
 * invocation.
 * 
 * Some invocations may be relevant to more than one Observer, but in general
 * they're only relevant to one.
 * **/
public enum Invocation {
    // Game Manager & Game Observer
    CITY_CAPTURED,
    // Game Manager
    TEAM_CFREATED,
    // Game Observer
    UNITS_SELECTED, UNITS_DESELECTED, DESTINATION_SET, 
    // UI Observer
    UNIT_MENU, CITY_MENU, TARGET_RING, CLOSE_ALL
}

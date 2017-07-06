using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Invocation {

    // Game Observer
    UNITS_SELECTED, UNITS_DESELECTED, DESTINATION_SET, CITY_CAPTURED,
    // UI Observer
    UNIT_MENU, CITY_MENU, TARGET_RING, CLOSE_ALL
}

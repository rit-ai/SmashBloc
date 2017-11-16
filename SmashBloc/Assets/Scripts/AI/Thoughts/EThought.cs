using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * An Enum containing the names of all classes that impelement IThought.
 * 
 * Please keep this file in alphabetical order, separated by the types of AIs
 * that can have these thoughts.
 * **/
public enum EThought {

    // PLAYER AIS
    SEND_MOBILE_TO_LOC,
    SPAWN_MOBILE,

    // TWIRL AIS
    SHOOT,
    FLEE,
    MOVE

}

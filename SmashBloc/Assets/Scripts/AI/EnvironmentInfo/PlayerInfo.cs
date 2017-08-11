using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to represent all the state that should be passed to a 
 * PlayerAI in order for it to evaluate its current situation and make a 
 * decision.
 * **/
public struct PlayerInfo {

    // **         //
    // * FIELDS * //
    //         ** //

    public Team team;
    public int goldAmount;

}

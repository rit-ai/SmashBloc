using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This abstract class is designed to contain state and denote methods common
 * between all thoughts a Player AI can receive.
 * **/
public abstract class PlayerThought : IThought{

    // **         //
    // * FIELDS * //
    //         ** //

    /// <summary>
    /// A PlayerThought will always manipulate a body of type Player.
    /// </summary>
    protected Player body;

    // **          //
    // * METHODS * //
    //          ** // 

    /// <summary>
    /// See IThought.
    /// </summary>
    public abstract void Act();

    public Player Body { set { body = value; } }

}

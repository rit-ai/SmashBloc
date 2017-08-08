using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This abstract class is designed to contain state and denote methods common
 * between all thoughts a Unit AI can have.
 * **/
public abstract class MobileThought : IThought {

    // **         //
    // * FIELDS * //
    //         ** //

    /// <summary>
    /// A MobileThought will always manipulate a MobileUnit object.
    /// </summary>
    protected MobileUnit body;

    // **          //
    // * METHODS * //
    //          ** // 

    /// <summary>
    /// See IThought.
    /// </summary>
    public abstract void Act();

    public MobileUnit Body { set { body = value; } }
}

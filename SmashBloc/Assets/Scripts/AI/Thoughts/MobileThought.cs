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

    protected MobileUnit body;

    public MobileUnit Body { set { body = value; } }

    public abstract void Act();
}

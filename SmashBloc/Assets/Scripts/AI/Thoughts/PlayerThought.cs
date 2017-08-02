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

    protected Player body;

    public Player Body { set { body = value; } }

    public abstract void Act();

}

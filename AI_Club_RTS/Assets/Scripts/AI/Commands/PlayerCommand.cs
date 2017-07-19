using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This abstract class is designed to contain state and denote methods common
 * between all Commands a Player AI can receive.
 * 
 * Command methods should not contain a parameter to replace
 * **/
public abstract class PlayerCommand : ICommand{

    protected Player body;

    public Player Body { set { body = value; } }

    public abstract void Execute();

}

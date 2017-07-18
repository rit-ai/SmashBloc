using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This abstract class is designed to contain state and denote methods common
 * between all Commands a Unit AI can receive.
 * **/
public abstract class MobileCommand : Command {

    protected MobileUnit body;

    public MobileUnit Body { set { body = value; } }

    public abstract void Execute();
}

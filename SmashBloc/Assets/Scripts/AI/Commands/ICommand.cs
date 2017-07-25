﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * A command is how Brains talk to Bodies. Once a Brain (AI) is done processing 
 * EnvironmentInfo, it will add one or more commands to the Body's event queue.
 * Commands are executed at a set rate that is common for all AI.
 * 
 * For example: A Unit sends information to its Brain that there are many 
 * enemies around. The Brain makes a decision to Flee and issues a FleeCommand
 * to its Body. The Body then flees away from the enemy units.
 * 
 * Any data needed by a Command will be passed in through its constructor. For
 * the above example, a FleeCommand would probably take in an array 
 * representation of enemies to flee from, as well as a distance of how far to
 * run by default.
 * 
 * Commands can overwrite each other. Once a Unit is far enough away from 
 * enemies such that it can no longer see any, the Brain could pass its Body an
 * IdleCommand, which would cause the Body to stop running.
 * **/
public interface ICommand {

    /// <summary>
    /// This function will contain more specific commands to provide to the 
    /// Body. In the above example, it would likely contain logic to calculate
    /// the direction in which to flee and correspondingly alter the Unit's
    /// current destination.
    /// 
    /// It is best practice to keep logic-heavy code in Execute in case the 
    /// Command Queue is cleared, in which case any logic-heavy code performed
    /// before execution is rendered useless.
    /// </summary>
    void Execute();

}
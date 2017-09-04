using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Thoughts are how Brains talk to Bodies. Once a Brain (AI) is done processing 
 * EnvironmentInfo, it will choose a command to execute. Info is sent and 
 * processed at a set, though asynchronous rate.
 * 
 * For example: A Unit sends information to its Brain that there are many 
 * enemies around. The Brain makes a decision to Flee and issues a FleeCommand
 * to its Body. The Body then flees away from the enemy units.
 * 
 * Any data needed by a Thought will be specified by its constructor. For the 
 * above example, a Flee thought would probably take in an array representation
 * of enemies to flee from, as well as a distance of how far to run by default.
 * **/
public interface IThought {

    /// <summary>
    /// This function will contain more specific commands to provide to the 
    /// Body. In the above example, it would likely contain logic to calculate
    /// the direction in which to flee and correspondingly alter the Unit's
    /// current destination.
    /// </summary>
    void Act();

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Abstract class that represents data common between the AI of different 
 * types. For instance, every AI must have a current state. AIs generally 
 * transitions between states according to an internal state machine that is 
 * defined based on the particular implementation.
 * **/
public abstract class BaseAI : MonoBehaviour {

    // This is the rate at which commands will be added to the commandQueue.
    protected const float COMMAND_ENQUEUE_RATE = 1f;

    // This is the rate at which commands will be passed to the body. Lower is
    // faster, but puts adds processing load.
    protected const float COMMAND_PROCESS_RATE = 1f;

    // This is the limits on the number of commands an AI's commandQueue can 
    // hold, based on the rate commands are processed.
    protected const int MAX_NUM_COMMANDS = (int)(10 / COMMAND_PROCESS_RATE);

    // This is the Queue of commands that the AI will feed back to its Body. 
    // Any number of commands can be enqueued, but they will be passed to the 
    // body at the rate of COMMAND_PASS_RATE.
    protected Queue<Command> commandQueue;

    // The current state of the AI. AIs can have any number of potential
    // states, but only one state can be active at a time. A AI's state handles
    // its frame-by-frame behavior—is it aggressively pursuing enemies, or is 
    // it running away? Is it regrouping, advancing, or retreating?
    protected State currentState;

    // This is a reference to the base object that the AI controls. Every AI
    // subclass should have a "wrapper" that makes the type of body more 
    // specific, while also restricting its usage so that any concrete classes
    // can only pass commands to the body.
    protected UnityEngine.Object body;

    /// <summary>
    /// Uses provided info to update the current state.
    /// </summary>
    /// At regular intervals, the AI must make a decision to update its current
    /// state. This is protected because child classes must specify a type of
    /// info that they will handle (see PlayerAI.cs for an example).
    /// <param name="info">Any information that may be relevant to the unit in 
    /// order for it to make a more effective decision, stored inside an info 
    /// struct. It is the responsibility of the implementer to make sure that 
    /// the struct is of the right type. </param>
    protected abstract void UpdateState(object info);

    /// <summary>
    /// Adds a command to the command queue.
    /// </summary>
    /// It is expected that child classes will make the Command parameter more 
    /// specific.
    /// <param name="command">The command to add.</param>
    protected abstract void AddCommand(Command command);

    /// <summary>
    /// Attempts to dequeue a command and execute it. Does nothing if there is
    /// no command.
    /// </summary>
    protected abstract IEnumerator ProcessNext();

    /// <summary>
    /// Sets up the executeCommand() IEnumerator, which executes every 
    /// COMMAND_PROCESS_RATE seconds.
    /// </summary>
    protected virtual void Start()
    {
        // Handle IEnumerators
        StartCoroutine(ProcessNext());

    }

}

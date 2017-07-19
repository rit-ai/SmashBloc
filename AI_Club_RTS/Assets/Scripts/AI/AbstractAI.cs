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
public abstract class AbstractAI : MonoBehaviour {

    // This is the rate at which the 'current command' variable will be checked
    // to see if a command is available for execution.
    protected const float COMMAND_PROCESS_RATE = 0.5f;

    // This is a reference to the base object that the AI controls. Every AI
    // subclass should have a "wrapper" that makes the type of body more 
    // specific, while also restricting its usage so that any concrete classes
    // can only pass commands to the body.
    protected GameObject body;

    // This is the next command that the Body will execute when ProcessNext() 
    // is called.
    protected ICommand currentCommand;

    /// <summary>
    /// Allows the body to provide updated info to the brain.
    /// </summary>
    /// At regular intervals, the body of the AI will call this function in 
    /// order to provide information. This is protected because child classes 
    /// must specify a type of info that they will handle (see PlayerAI.cs for 
    /// an example).
    /// <param name="info">Any information that may be relevant to the unit in 
    /// order for it to make a more effective decision, stored inside an info 
    /// struct. It is the responsibility of the implementer to make sure that 
    /// the struct is of the right type. </param>
    public abstract void UpdateInfo(object info);

    /// <summary>
    /// Adds a command to the command queue.
    /// </summary>
    /// It is expected that child classes will make the Command parameter more 
    /// specific.
    /// <param name="command">The command to add.</param>
    protected abstract void SetCurrentCommand(ICommand command);

    /// <summary>
    /// Attempts to execute the current command. Does nothing if there is no
    /// command.
    /// </summary>
    protected abstract IEnumerator ProcessNext();

    /// <summary>
    /// Chooses a Command to add to the CommandQueue based on a weighted 
    /// distribution of values mapped to Commands.
    /// </summary>
    /// <param name="weightedCommands"></param>
    /// <returns></returns>
    protected void Prioritize(Dictionary<float, ICommand> weightedCommands)
    {
        // Take the total of all the keys.
        float total = 0f;
        foreach (float v in weightedCommands.Keys)
        {
            total += v;
        }
        // Use the total to determine the max random value.
        float rand = UnityEngine.Random.Range(0f, total);
        total = 0f;
        // Higher values are more likely to be chosen.
        foreach (float v in weightedCommands.Keys)
        {
            total += v;
            if (rand <= total) { SetCurrentCommand(weightedCommands[v]); }
        }
        throw new Exception("Unreachable value in AbstractAI.Prioritize()!");
    }

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

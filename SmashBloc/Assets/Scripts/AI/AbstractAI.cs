using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Abstract class that represents data common between the AI of different 
 * types.
 * **/
public abstract class AbstractAI : MonoBehaviour {

    // This is a reference to the base object that the AI controls. Every AI
    // subclass should have a "wrapper" that makes the type of body more 
    // specific, while also restricting its usage. See MobileAI for details.
    protected GameObject body;

    // This is the most recent command that the Body executed. Useful if you
    // don't want to execute the same type of command again and again.
    protected ICommand mostRecentCommand;

    /// <summary>
    /// Allows the body to provide updated info to the brain.
    /// </summary>
    /// At regular intervals, the body of the AI will call this function in 
    /// order to provide information. This is protected because child classes 
    /// must specify a type of info that they will handle (see MobileAI.cs for 
    /// an example).
    /// <param name="info">Any information that may be relevant to the unit in 
    /// order for it to make a more effective decision, stored inside an info 
    /// struct. It is the responsibility of the implementer to make sure that 
    /// the struct is of the right type. </param>
    public abstract void UpdateInfo(object info);

    /// <summary>
    /// Decide how to handle new information. Will be called after every update
    /// to info.
    /// </summary>
    protected abstract ICommand Decide();

    /// <summary>
    /// Attempts to execute the current command. Does nothing if currentCommand
    /// is null, as that implies the body hasn't been given orders.
    /// </summary>
    protected virtual void Behave(ICommand command)
    {
        if (command != null)
        {
            command.Execute();
            mostRecentCommand = command;
            command = null;
        }
    }

    /// <summary>
    /// Chooses a Command to add to the CommandQueue based on a weighted 
    /// distribution of values mapped to Commands.
    /// </summary>
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
            if (rand <= total) { Behave(weightedCommands[v]); }
        }
        throw new Exception("Unreachable value in AbstractAI.Prioritize()!");
    }

}

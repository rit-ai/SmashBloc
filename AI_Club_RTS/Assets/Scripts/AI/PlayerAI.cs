using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class representing the most basic form of Player AI and providing details on
 * how to implement a custom one. The basic dataflow of a Player AI is as 
 * follows:
 * 
 * 1. Receive Info from Body
 * 2. Process Info and form Commands
 * 3. Enqueue Commands
 * 4. Wait for more Info
 * 
 * Step one is called by the body in UpdateInfo(). Step two is Decide(). Step
 * three is AddCommand(). Children classes only have to worry about 
 * implementing Decide(), building Commands, and calling AddCommand(). 
 * Everything else is handled externally.
 * **/
public abstract class PlayerAI : AbstractAI {

    // Player AIs control Players. However, the AIs aren't allowed to reference
    // their Players themselves—they can only do so through Commands.
    new private Player body;
    // This is the most recent information the AI has from its body.
    protected PlayerInfo info;

    public Player Body { set { body = value; } }

    /// <summary>
    /// Decide how to handle new information. Will be called after every update
    /// to info.
    /// </summary>
    protected abstract void Decide();

    /// <summary>
    /// Allows the body to send updated information to the brain.
    /// </summary>
    /// <param name="info">Updated information about the Player's status, 
    /// contained in a PlayerInfo class.</param>
    public sealed override void UpdateInfo(object info)
    {
        if (!(info is PlayerInfo))
        {
            throw new ArgumentException("Attempted to call UpdateState with wrong Info type.", "info");
        }
        this.info = (info as PlayerInfo);
        Decide();
    }

    // Protected and sealed to satisfy the base class
    protected sealed override void SetCurrentCommand(ICommand command)
    {
        if (!(command is PlayerCommand))
        {
            throw new ArgumentException("Attempted to call AddCommand with wrong Command type.", "command");
        }
        AddCommand(command as PlayerCommand);
    }

    /// <summary>
    /// Adds a command to the command queue.
    /// </summary>
    /// <param name="command">The command to add.</param>
    protected void AddCommand(PlayerCommand command)
    {
        command.Body = body;
        currentCommand = command;
    }

    /// <summary>
    /// Executes the next command in the queue, if one is present.
    /// </summary>
    protected sealed override IEnumerator ProcessNext()
    {
        while (true)
        {
            if (currentCommand != null)
            {
                currentCommand.Execute();
                currentCommand = null;
            }
            yield return new WaitForSeconds(COMMAND_PROCESS_RATE);
        }
    }


}

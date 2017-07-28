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
 * 2. Process Info and execute ICommand
 * 
 * Step one is called by the body in UpdateInfo(). Step two is Decide(). 
 * Children classes only have to worry about implementing Decide().
 * **/
public abstract class PlayerAI : AbstractAI {

    // Player AIs control Players. However, the AIs aren't allowed to reference
    // their Players themselves—they can only do so through Commands.
    private new Player body;
    // This is the most recent information the AI has from its body.
    protected PlayerInfo info;

    public Player Body { set { body = value; } }

    /// <summary>
    /// Allows the body to send updated information to the brain, then 
    /// immediately uses that information to execute a command.
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
        Behave(Decide());
    }

    /// <summary>
    /// Sets the current command for execution.
    /// </summary>
    protected override sealed void Behave(ICommand command)
    {
        if (command == null) { return; }

        if (!(command is PlayerCommand))
        {
            throw new ArgumentException("Wrong command type.");
        }
        
        ((PlayerCommand)command).Body = body;
        base.Behave(command);
    }


}

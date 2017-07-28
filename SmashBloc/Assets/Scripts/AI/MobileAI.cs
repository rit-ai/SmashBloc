using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class created to represent state common across all Unit AIs and providing 
 * details on how to implement a custom one. The basic dataflow of a Mobile AI 
 * is as follows:
 * 
 * 1. Receive Info from Body
 * 2. Process Info and execute ICommand
 * 
 * Step one is called by the body in UpdateInfo(). Step two is Decide(). 
 * Children classes only have to worry about implementing Decide().
 * **/
public abstract class MobileAI : AbstractAI
{
    // Unit AIs control Units. However, the AIs aren't allowed to reference
    // their Units themselves—they can only do so through Commands.
    private new MobileUnit body;
    // This is the most updated information the AI has from its body.
    protected MobileUnitInfo info;

    public MobileUnit Body { set { body = value; } }

    /// <summary>
    /// Allows the body to send updated information to the brain, then 
    /// immediately uses that information to execute a command.
    /// </summary>
    /// <param name="info">Updated information about the Mobiles's status, 
    /// contained in a MobileInfo class.</param>
    public sealed override void UpdateInfo(object info)
    {
        if (!(info is MobileUnitInfo))
        {
            throw new ArgumentException("Attempted to call UpdateState with wrong Info type.", "info");
        }
        this.info = (info as MobileUnitInfo);
        Behave(Decide());
    }

    /// <summary>
    /// Sets the current command for execution.
    /// </summary>
    protected override sealed void Behave(ICommand command)
    {
        if (command == null) { return; }

        if (!(command is MobileCommand))
        {
            throw new ArgumentException("Wrong command type.");
        }

        ((MobileCommand)command).Body = body;
        base.Behave(command);
    }
}

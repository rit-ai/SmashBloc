using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class created to represent state common across all Unit AIs and providing 
 * details on how to implement a custom one. The basic dataflow of a Unit AI is
 * as follows:
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
public abstract class UnitAI : BaseAI
{
    // Unit AIs control Units. However, the AIs aren't allowed to reference
    // their Units themselves—they can only do so through Commands.
    new private Unit body;
    // Unit AIs command Players with UnitCommands.
    new protected Queue<UnitCommand> commandQueue;
    // This is the most updated information the AI has from its body.
    protected MobileUnitInfo info;
    // The absolute destination of the unit, separate from the local 
    // destination (which this AI should freely change). This value is set by 
    // either the player, or by a "parent AI" that controls all the units.
    protected Vector3 absoluteDest;

    public Unit Body { set { body = value; } }

    /// <summary>
    /// Decide how to handle new information. Will be called after every update
    /// to info.
    /// </summary>
    protected abstract void Decide();

    // Sealed and protected, to handle the requirements of BaseAI
    public sealed override void UpdateInfo(object info)
    {
        if (!(info is MobileUnitInfo))
        {
            throw new ArgumentException("Attempted to call UpdateState with wrong Info type.", "info");
        }
        this.info = (info as MobileUnitInfo);
        Decide();
    }

    // Sealed and protected, to handle the requirements of BaseAI
    protected sealed override void AddCommand(Command command)
    {
        if (!(command is UnitCommand))
        {
            throw new ArgumentException("Attempted to call AddCommand with wrong Command type.", "command");
        }
        AddCommand(command as UnitCommand);
    }

    /// <summary>
    /// Enqueues a command to the commandQueue.
    /// </summary>
    /// <param name="command">The command to enqueue.</param>
    protected void AddCommand(UnitCommand command)
    {
        while (commandQueue.Count >= MAX_NUM_COMMANDS) { return; }
        command.Body = body;
        commandQueue.Enqueue(command);
    }


    // Sealed and protected, to handle the requirements of BaseAI
    protected sealed override IEnumerator ProcessNext()
    {
        while (true)
        {
            while (commandQueue.Count == 0) { yield return new WaitForSeconds(COMMAND_PROCESS_RATE); }
            Command command = commandQueue.Dequeue();
            if (!(command is UnitCommand))
            {
                throw new ArgumentException("Attempted to call AddCommand with wrong Command type.", "command");
            }
            command.Execute();
            yield return new WaitForSeconds(COMMAND_PROCESS_RATE);
        }
    }

    protected new virtual void Start()
    {
        commandQueue = new Queue<UnitCommand>();

        base.Start();
    }
}

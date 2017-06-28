using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class created to represent state common across all AIs that control Units.
 * **/
public abstract class UnitAI : BaseAI
{ 
    // Unit AIs command Players with UnitCommands.
    new protected Queue<UnitCommand> commandQueue;

    // The absolute destination of the unit, separate from the local 
    // destination (which this AI should freely change). This value is set by 
    // either the player, or by a "parent AI" that controls all the units.
    protected Vector3 absoluteDest;

    // Unit AIs control Units. However, the AIs aren't allowed to reference
    // their Units themselves—they can only do so through Commands.
    new private Unit body;

    public Unit Body { set { body = value; } }

    // From interface -- accept information and update state if necessary
    public abstract void UpdateState(UnitInfo info);

    // Sealed and protected, to handle the requirements of BaseAI
    protected sealed override void UpdateState(object info)
    {
        if (!(info is UnitInfo))
        {
            throw new ArgumentException("Attempted to call UpdateState with wrong Info type.", "info");
        }
        UpdateState(info as UnitInfo);
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
        commandQueue.Enqueue(command);
    }


    // Sealed and protected, to handle the requirements of BaseAI
    protected sealed override IEnumerator ProcessNext()
    {
        while (true)
        {
            while (commandQueue.Count == 0) { yield return COMMAND_PROCESS_RATE; }
            Command command = commandQueue.Dequeue();
            if (!(command is UnitCommand))
            {
                throw new ArgumentException("Attempted to call AddCommand with wrong Command type.", "command");
            }
            command.Execute();
            yield return COMMAND_PROCESS_RATE;
        }
    }

    protected new virtual void Start()
    {
        commandQueue = new Queue<UnitCommand>();

        base.Start();
    }
}

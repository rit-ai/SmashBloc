using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class representing the most basic form of Player AI and providing details on
 * how to implement a custom one.
 * **/
public abstract class PlayerAI : BaseAI {

    // Player AIs control Players. However, the AIs aren't allowed to reference
    // their Players themselves—they can only do so through Commands.
    new private Player body;
    // Player AIs command Players with PlayerCommands.
    new private Queue<PlayerCommand> commandQueue;

    public Player Body { get; set; }

    // Update the state using information specific to the Player
    public abstract void UpdateState(PlayerInfo info);

    // Sealed and protected, to handle the requirements of BaseAI
    // Command queue is cleared every time new information is presented
    protected sealed override void UpdateState(object info)
    {
        if (!(info is PlayerInfo))
        {
            throw new ArgumentException("Attempted to call UpdateState with wrong Info type.", "info");
        }
        UpdateState(info as PlayerInfo);
    }

    // Protected and sealed to satisfy the base class
    protected sealed override void AddCommand(Command command)
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
        if (commandQueue.Count >= MAX_NUM_COMMANDS) { return; }
        commandQueue.Enqueue(command);
    }

    /// <summary>
    /// Processes the next command in the queue, if one is present.
    /// 
    /// //TODO Why are commands enqueued so rapidly?
    /// </summary>
    protected sealed override IEnumerator ProcessNext()
    {
        while (true)
        {
            Debug.Log(commandQueue.Count);
            while (commandQueue.Count == 0) { yield return COMMAND_PROCESS_RATE; }
            Command command = commandQueue.Dequeue();
            if (!(command is PlayerCommand))
            {
                throw new ArgumentException("Attempted to call AddCommand with wrong Command type.", "command");
            }
            command.Execute();
            yield return COMMAND_PROCESS_RATE;
        }
    }

    protected override void Start()
    {
        commandQueue = new Queue<PlayerCommand>();

        base.Start();
    }


}

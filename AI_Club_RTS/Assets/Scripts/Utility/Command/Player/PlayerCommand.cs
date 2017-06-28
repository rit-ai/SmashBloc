using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCommand : Command{

    protected Player body;

    public PlayerCommand(Player body)
    {
        this.body = body;
    }

    public abstract void Execute();

}

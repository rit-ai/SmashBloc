using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitCommand : Command {

    private Unit body;

    public UnitCommand(Unit body)
    {
        this.body = body;
    }

    public abstract void Execute();
}

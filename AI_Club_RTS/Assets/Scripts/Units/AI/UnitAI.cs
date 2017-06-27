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
    public Unit body;

    public Unit Body
    {
        set { body = value; }
    }

    public virtual void Start()
    {

    }

    public abstract override void UpdateState(object info);
}

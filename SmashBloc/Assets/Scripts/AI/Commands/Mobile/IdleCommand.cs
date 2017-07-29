using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This command isn't really anything. It just tells the Unit to move in a 
 * random radius around an anchor.
 * **/
public class IdleCommand : MobileCommand
{
    private const float MIN_MOVE = 10f;
    private const float MAX_MOVE = 50f;

    private Vector3 anchor;

    public IdleCommand(Vector3 anchor) {
        this.anchor = anchor;
    }

    public override void Execute()
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(MIN_MOVE, MAX_MOVE);
        Vector3 newDest = new Vector3(anchor.x + offset.x, anchor.y, anchor.z + offset.y);
        body.Destination = newDest;
    }
}

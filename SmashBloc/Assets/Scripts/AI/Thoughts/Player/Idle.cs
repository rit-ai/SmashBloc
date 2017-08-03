using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Currently a placeholder for idling behavior.
 * **/
public class Idle : PlayerThought {

    public override void Act()
    {
        Debug.Log("Player is idling!");
    }

}

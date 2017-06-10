using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {

    /// <summary>
    /// Handles any user input from implementer.
    /// </summary>
    public abstract void HandleInput();

    /// <summary>
    /// Handles Update() delegation from implementer.
    /// 
    /// This is the function that will be called instead of Update() when a 
    /// class implementing this pattern needs to update.
    /// </summary>
    public abstract void StateUpdate();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State {

    /// <summary>
    /// Handles any user input from implementer.
    /// </summary>
    void HandleInput();

    /// <summary>
    /// Handles Update() delegation from implementer.
    /// 
    /// This is the function that will be called instead of Update() when a 
    /// class implementing this pattern needs to update.
    /// </summary>
    void StateUpdate();
}

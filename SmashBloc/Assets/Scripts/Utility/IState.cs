using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState {

    /// <summary>
    /// Handles any work that the implementer must do while in a particular 
    /// State. It should execute every EXECUTION_RATE seconds. Lower is faster,
    /// but increases CPU load.
    /// </summary>
    void StateUpdate();
}

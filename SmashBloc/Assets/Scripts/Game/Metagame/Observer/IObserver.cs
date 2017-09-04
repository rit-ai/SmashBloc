using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Interface designed to represent classes that observe the behavior of 
 * objects and handle cases where they must interact with each other. By hiding
 * this interaction behind the Observer, the two interacting classes can stay
 * more effectively decoupled, keeping the scripts more maintainable in the 
 * long run.
 * **/
public interface IObserver {

    /// <summary>
    /// How should the Observer behave when it is notified?
    /// </summary>
    /// <param name="entity">The entity notifying the Observer.</param>
    /// <param name="invocation">The type of invocation.</param>
    /// <param name="data">Any additional data or state that may be useful.</param>
    void OnNotify(object entity, Invocation invoke, params object[] data);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Interface designed to represent classes that observe the behavior of other,
 * unrelated objects and establish functionality outside the scope of said
 * classes.
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

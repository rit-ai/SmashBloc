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
public interface Observer {

    /// <summary>
    /// How should the Observer behave when it is notified?
    /// </summary>
    /// <param name="entity">The entity notifying the Observer.</param>
    /// <param name="data">Any additional data or state that may be useful.</param>
    void OnNotify(Object entity, string data);

}

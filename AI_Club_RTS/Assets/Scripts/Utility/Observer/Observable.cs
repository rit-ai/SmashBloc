using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * A partner class to Observer, this interface should be applied when a class's
 * behavior impacts data outside its domain. In order to communicate that 
 * change, Observers receive notifications and forward them to relevant 
 * classes.
 * **/
public interface Observable {

    /// <summary>
    /// Notify all observers.
    /// </summary>
    /// <param name="invocation">The title of the event.</param>
    /// <param name="data">Any other useful data.</param>
    void NotifyAll<T>(string invocation, params T[] data);

}

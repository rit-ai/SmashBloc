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
    /// <param name="data">A generic piece of data that tells the observer what
    /// event is actually occurring.</param>
    void NotifyAll<T>(T data);

}

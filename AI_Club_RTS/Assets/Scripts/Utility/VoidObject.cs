using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * A debug object with no content. To be used in place of <<dynamic>> in the case
 * where a typename must be passed to a generic funtion with optional arguments.
 * Naturally this type should never be instantiated or accessed.
 * 
 * (AKA I should probably be using Python.)
 * **/
public class VoidObject {

    /// <summary>
    /// Should never be instantiated.
    /// </summary>
    public VoidObject()
    {
        throw new System.Exception("Tried to instantiate VoidObject!");
    }

}

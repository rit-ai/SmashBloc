using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to decouple the different domains of a single class, e.g.
 * decouple a Unit's Physics from its AI behavior.
 * **/
public interface RTS_Component {

    /// <summary>
    /// Handle the behavior, and update the state, of this component.
    /// </summary>
    /// <param name="data">Any additional useful data, if any. Optional.</param>
    void ComponentUpdate();

}

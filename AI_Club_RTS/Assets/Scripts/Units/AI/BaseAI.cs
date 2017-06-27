using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Abstract class that represents data common between the AI of different 
 * units.
 * **/
public interface BaseAI : RTS_Component {

    /// <summary>
    /// At regular intervals, the AI must make a decision to update its current
    /// state.
    /// </summary>
    /// <param name="info">Any information that may be relevant to the unit in 
    /// order for it to make a more effective decision, stored inside an 
    /// EnvironmentInfo struct. </param>
    void UpdateState(EnvironmentInfo info);

}

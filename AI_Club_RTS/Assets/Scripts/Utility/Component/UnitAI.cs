using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Abstract class that represents data common between the AI of different 
 * units.
 * **/
public abstract class UnitAI : Component {

    // Protected fields
    // The unit's parent, for reference purposes
    protected Unit m_Parent;
    // The absolute destination of the unit, separate from the 
    protected Vector3 absoluteDest;
    // Whether or not the local destination has recently been changed
    protected bool destChanged;

    public void SetDestChanged()
    {
        destChanged = true;
    }

    public abstract void ComponentUpdate();

}

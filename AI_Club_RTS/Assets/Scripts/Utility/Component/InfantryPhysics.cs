using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to handle the physics of the Infantry Unit.
 * **/
public class InfantryPhysics : Component {

    // This class should have no public state or methods besides its 
    // constructor and ComponentUpdate().

    // Private constants
    private const float MIN_FLOAT_DISTANCE = 10f;

    // Private fields
    private Infantry m_Parent;
    private Rigidbody m_Hoverball;

    public InfantryPhysics(Infantry parent)
    {
        Debug.Assert(parent is Infantry);
        m_Parent = parent;
        m_Hoverball = parent.m_Hoverball;
    }

    /// <summary>
    /// Blanket to update all private methods.
    /// </summary>
    public void ComponentUpdate()
    {
        Hover();
    }

    /// <summary>
    /// Infantry Units hover above the ground, based on their current distance 
    /// from the floor. 
    /// </summary>
    private void Hover()
    {
        // Use a Raycast to hit the floor from the 
        // The farther a unit is from the floor, the less upward force is applied.
        RaycastHit hit;
        if (Physics.Raycast(m_Hoverball.transform.position, Vector3.down, out hit, MIN_FLOAT_DISTANCE))
        {
            Vector3 hoverQuotient = Vector3.up * Mathf.Abs(Physics.gravity.y) * m_Hoverball.mass;
            hoverQuotient = hoverQuotient / (hit.distance);
            m_Hoverball.AddForce(hoverQuotient, ForceMode.Acceleration);
        }
    }

    private void Guide()
    {

    }

}

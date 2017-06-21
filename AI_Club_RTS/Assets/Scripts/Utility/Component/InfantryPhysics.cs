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
    private const float MIN_FLOAT_DISTANCE = 15f;
    private const float MAX_VECTOR_FORCE = 100f;

    // Private fields
    private Infantry m_Parent;
    private Rigidbody m_Hoverball;
    private Rigidbody m_BottomWeight;

    public InfantryPhysics(Infantry parent)
    {
        Debug.Assert(parent is Infantry);
        m_Parent = parent;
        m_Hoverball = parent.m_Hoverball;
        m_BottomWeight = parent.m_BottomWeight;
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
    /// from the floor. They will attempt to hover toward the parent's 
    /// destination. The Hoverball provides upward force. The BottomWeight
    /// provides stability (so that the Infantry doesn't constantly flip and
    /// spin around).
    /// </summary>
    private void Hover()
    {
        // The farther it is from the floor, the less upward force is applied.
        RaycastHit hit;
        // If the unit is too far from the floor, don't apply any force to the
        // hoverball
        if (Physics.Raycast(m_Hoverball.transform.position, Vector3.down, out hit, MIN_FLOAT_DISTANCE))
        {
            // Get a generic UP vector
            Vector3 hoverForce = Vector3.up * Mathf.Abs(Physics.gravity.y) * m_Hoverball.mass;
            // The further the distance from the floor, the less force is applied
            hoverForce = hoverForce / hit.distance;
            // Get the vector in the direction of the destination
            Vector3 distanceToDest = (m_Parent.Destination - m_Parent.transform.position);
            // Get rid of the Y factor so that the hover stays the same
            distanceToDest.y = 0;
            // Adjust by that vector
            hoverForce += distanceToDest;
            // Cap the amount of force (to prevent strange launches)
            hoverForce = Vector3.ClampMagnitude(hoverForce, MAX_VECTOR_FORCE);
            // Add the force
            m_Hoverball.AddForce(hoverForce, ForceMode.Acceleration);
        }

        // Add downward force to the bottom weight and resist changes in motion
        Vector3 downForce = Vector3.down * Mathf.Abs(Physics.gravity.y) * m_BottomWeight.mass;
        m_BottomWeight.AddForce(downForce, ForceMode.Acceleration);
    }

}

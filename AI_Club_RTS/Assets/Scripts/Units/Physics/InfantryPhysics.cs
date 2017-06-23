using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to handle the physics of the Infantry Unit.
 * **/
public class InfantryPhysics : RTS_Component {

    // This class should have no public state or methods besides its 
    // constructor and ComponentUpdate().

    // Private constants
    // Height above ground at which float force is applied
    private const float MAX_FLOAT_THRESHOLD = 10f;
    // Distance from destination at which deceleration begins (squared)
    private const float DECELERATION_THRESHOLD_SQRD = 1000f;
    // Max force that can be applied to a rigidbody
    private const float MAX_VECTOR_FORCE = 100f;
    private const float MAX_GUIDING_FORCE = 50f;
    // Higher increases speed threshhold before deceleration begins
    private const float MAX_SPEED = 75f;
    // Higher resists motion more (ONLY BETWEEN 0-1)
    private const float RESISTANCE_FACTOR = 0.7f;

    // Private fields
    private Infantry m_Parent;
    private Rigidbody m_Parent_Rigidbody;
    private Rigidbody m_Hoverball;
    private Rigidbody m_BottomWeight;

    public InfantryPhysics(Infantry parent)
    {
        Debug.Assert(parent is Infantry);
        m_Parent = parent;

        // Private fields
        m_Parent_Rigidbody = parent.GetComponent<Rigidbody>();
        m_Hoverball = parent.m_Hoverball;
        m_BottomWeight = parent.m_BottomWeight;
    }

    /// <summary>
    /// Blanket to update all private methods.
    /// </summary>
    public void ComponentUpdate()
    {
        Hover();
        Guide();
    }

    /// <summary>
    /// Infantry Units hover above the ground, based on their current distance 
    /// from the floor. They will attempt to hover toward the parent's 
    /// destination and orbit around it.
    /// 
    /// The Hoverball provides upward force. The BottomWeight provides 
    /// stability (so that the Infantry doesn't constantly flip and spin 
    /// around).
    /// </summary>
    private void Hover()
    {
        // The farther it is from the floor, the less upward force is applied.
        RaycastHit hit;
        // If the unit is too far from the floor, don't apply any force to the
        // hoverball
        if (Physics.Raycast(m_Hoverball.transform.position, Vector3.down, out hit, MAX_FLOAT_THRESHOLD))
        {
            // Get the destination height
            Vector3 destination = Vector3.up * MAX_FLOAT_THRESHOLD * Mathf.Abs(Physics.gravity.y);
            // Adjust UP by the masses of the hoverball and parent rigidbody
            Vector3 desire = Vector3.up * (destination.y - m_Parent_Rigidbody.velocity.y * MAX_GUIDING_FORCE);

            // Cap the amount of force (to prevent strange launches)
            desire = Vector3.ClampMagnitude(desire, MAX_VECTOR_FORCE);
            // Apply force
            m_Hoverball.AddForce(desire, ForceMode.Acceleration);
        }

        // Add downward force to the bottom weight and resist changes in motion
        Vector3 downForce = Vector3.down * Mathf.Abs(Physics.gravity.y) * m_BottomWeight.mass;
        m_BottomWeight.AddForce(downForce, ForceMode.Acceleration);
    }

    /// <summary>
    /// Guides the hoverball toward the destination.
    /// </summary>
    private void Guide()
    {
        // Get the vector representing the desired trajectory
        Vector3 desire = m_Parent.Destination - m_Parent.transform.position;
        // Store the magnitude for later use
        float desireMagnitude = desire.sqrMagnitude;
        // Normalize and adjust the vector to use max speed by default
        desire = desire.normalized * MAX_SPEED;
        // Lower that speed depending on its distance from the destination
        desire *= Decelerate(desireMagnitude / DECELERATION_THRESHOLD_SQRD);
        // Steering = desired - velocity
        desire -= m_Parent_Rigidbody.velocity;
        // Get rid of the Y factor so that the hover stays the same
        desire.y = 0;
        // Cap the amount of force (to prevent strange launches)
        desire = Vector3.ClampMagnitude(desire, MAX_GUIDING_FORCE);
        // Add the force
        m_Hoverball.AddForce(desire, ForceMode.Acceleration);
    }

    /// <summary>
    /// Appoximates the deceleration force based on ratio of the current 
    /// distance to the destination. Formula is as follows:
    /// 
    /// 1.442 * ln(^3root(x) + 1)
    /// 
    /// Or you can paste this into desmos.com/calculator :
    /// 
    /// 1.442\ln \left(\sqrt[3]{x}\ +\ 1\right)
    /// </summary>
    /// <param name="distanceToDestRatio">The ratio of the current position of
    /// the object to its desired position.</param>
    /// <returns></returns>
    private float Decelerate(float distanceToDestRatio)
    {
        const float SCALE_FACTOR = 1.442f; // DO NOT CHANGE
        return Mathf.Min(SCALE_FACTOR * Mathf.Log(Mathf.Pow(distanceToDestRatio + 1f, 0.33333333f), Mathf.Exp(1)), 1f);
    }

}

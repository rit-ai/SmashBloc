using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to handle the physics of the Twirl Unit. The functions in 
 * this file are designed in such a way to reasonably accomodate being called
 * many times a second. Being that this design precludes the use of conditional
 * branches, it may be a bit confusing to read.
 * **/
public class TwirlPhysics : MobilePhysics {

    // This class should have no public state or methods besides its 
    // constructor and ComponentUpdate().

    // Thresholds for unit convergence and separation
    private const float MAX_DISTANCE_FROM = 400f; // "sight range"
    private const float MIN_DISTANCE_FROM_SQR = 64f;
    // Height above ground at which float force is applied
    private const float MAX_FLOAT_THRESHOLD = 20f;
    // Distance from destination at which deceleration begins (squared)
    private const float DECELERATION_THRESHOLD_SQRD = 1000f;
    // Max force that can be applied to a rigidbody
    private const float MAX_VECTOR_FORCE = 200f;
    private const float UP_FORCE = 100f;
    // Default steering force strength
    private const float SPEED = 200f;
    // Adjusts the intensity of steering forces
    private const float GUIDANCE_FACTOR = 1f;
    private const float CONVERGE_FACTOR = 0.25f;
    private const float DIVERGE_FACTOR = 0.5f;
    private const float ALIGNMENT_FACTOR = 0.75f;


    // Private fields
    private Twirl m_Parent;
    private Rigidbody m_Rigidbody;
    private Rigidbody m_Hoverball;
    private Rigidbody m_BottomWeight;

    private List<Collider> withinSight;
    private List<Collider> withinMaxDistance;
    private List<Collider> withinMinDistance;

    private Vector3 converge;
    private Vector3 diverge;
    private Vector3 align;

    private void Start()
    {
        // Private fields
        m_Parent = GetComponent<Twirl>();
        m_Rigidbody = m_Parent.GetComponent<Rigidbody>();
        m_Hoverball = m_Parent.m_Hoverball;
        m_BottomWeight = m_Parent.m_BottomWeight;

        m_Rigidbody.useGravity = true;
        m_BottomWeight.useGravity = true;
    }

    /// <summary>
    /// Blanket to update all private methods.
    /// </summary>
    protected override void Navigate()
    {
        // Only perform more complex behaviors if we're close to the ground
        if (Hover())
        {
            Guide();
            Flock();
        }
    }

    /// <summary>
    /// Twirl Units hover above the ground, based on their current distance 
    /// from the floor. They will attempt to hover toward the parent's 
    /// destination and orbit around it.
    /// 
    /// The Hoverball provides upward force. The BottomWeight provides 
    /// stability (which tempers how often the Twirl flips around).
    /// </summary>
    /// <returns>True if the Twirl is close enough to a surface to hover.
    /// </returns>
    private bool Hover()
    {
        // The farther it is from the floor, the less upward force is applied.
        RaycastHit hit;

        // Add downward force to the bottom weight and resist changes in motion
        Vector3 downForce = Vector3.down * m_BottomWeight.mass;
        m_BottomWeight.AddForce(downForce, ForceMode.Acceleration);

        // If the unit is too far from the floor, don't apply any force to the
        // hoverball
        if (Physics.Raycast(m_Hoverball.transform.position, Vector3.down, out hit, MAX_FLOAT_THRESHOLD, Toolbox.Terrain.ignoreAllButTerrain))
        {
            // Get the destination height
            Vector3 destination = Vector3.up * MAX_FLOAT_THRESHOLD * Mathf.Abs(Physics.gravity.y);
            // Adjust UP by the masses of the hoverball and parent rigidbody
            Vector3 desire = Vector3.up * (destination.y - m_Rigidbody.velocity.y * UP_FORCE);
            desire -= m_Hoverball.velocity;
            // Cap the amount of force (to prevent strange launches)
            desire = Vector3.ClampMagnitude(desire, MAX_VECTOR_FORCE);
            // Apply force
            m_Hoverball.AddForce(desire, ForceMode.Acceleration);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Guides the hoverball toward the destination, while avoiding other 
    /// hoverballs.
    /// </summary>
    private void Guide()
    {
        // Get the vector representing the desired trajectory
        Vector3 desire = m_Parent.Destination - m_Parent.transform.position;
        // Store the magnitude for later use
        float desireMagnitude = desire.sqrMagnitude;
        // Get rid of the Y factor so that the hover stays the same
        desire.y = 0;
        // Normalize and adjust the vector to use max speed by default
        desire = desire.normalized * SPEED * GUIDANCE_FACTOR;
        // Lower that speed depending on its distance from the destination
        desire *= Decelerate(desireMagnitude / DECELERATION_THRESHOLD_SQRD);
        // Steering = desire - velocity
        Vector3 velocity = m_Rigidbody.velocity;
        velocity.y = 0;
        desire -= velocity;
        // Cap the amount of force (to prevent strange launches)
        desire = Vector3.ClampMagnitude(desire, MAX_VECTOR_FORCE);
        // Reduce the amount of force if not hovering
        // Add the force
        m_Hoverball.AddForce(desire, ForceMode.Acceleration);
    }

    /// <summary>
    /// Hoverballs will try to stay within a certain radius of each other, but 
    /// also outside a certain radius of each other, and also generally go in
    /// the same direction as others.
    /// </summary>
    /// <returns>A normalized Vector3 that accounts for both convergent, 
    /// divergent and alignment forces.</returns>
    private void Flock()
    {
        Unit current;
        // Get all units that are within a very small "sight" range.
        withinSight = new List<Collider>(Physics.OverlapSphere(transform.position, MAX_DISTANCE_FROM, m_Parent.ignoreAllButMobiles));
        withinMaxDistance = new List<Collider>();
        withinMinDistance = new List<Collider>();
        foreach (Collider c in withinSight)
        {
            current = c.gameObject.GetComponent<Unit>();
            // Only keep allies in the list.
            if (current.Team == m_Parent.Team)
            {
                // Move allies that are too close into another list.
                if ((c.transform.position - m_Parent.transform.position).sqrMagnitude < MIN_DISTANCE_FROM_SQR)
                {
                    withinMinDistance.Add(c);
                }
                else
                {
                    withinMaxDistance.Add(c);
                }
            }
        }

        // Multiply the components of the convergent force by the components of
        // the divergent force and normalize the result.
        converge = Converge(withinMaxDistance).normalized;
        diverge = Diverge(withinMinDistance).normalized;
        align = Align(withinSight).normalized;

        converge = Vector3.ClampMagnitude(converge * SPEED * CONVERGE_FACTOR, MAX_VECTOR_FORCE);
        diverge = Vector3.ClampMagnitude(diverge * SPEED * DIVERGE_FACTOR, MAX_VECTOR_FORCE);
        align = Vector3.ClampMagnitude(align * SPEED * ALIGNMENT_FACTOR, MAX_VECTOR_FORCE);

        m_Rigidbody.AddForce(converge, ForceMode.Acceleration);
        m_Rigidbody.AddForce(diverge, ForceMode.Acceleration);
        m_Rigidbody.AddForce(align, ForceMode.Acceleration);
    }

    /// <summary>
    /// Returns a vector representing the direction of the general center of 
    /// the colliders in goToward.
    /// </summary>
    private Vector3 Converge(List<Collider> goToward)
    {
        if (goToward.Count == 0) { return Vector3.zero; }
        Vector3 result = Vector3.zero;
        foreach (Collider c in goToward)
        {
            result += c.transform.position;
        }
        result.y = 0;
        return (m_Parent.transform.position - result);
    }

    /// <summary>
    /// Returns a  vector in the general direction away from the colliders in 
    /// goAwayFrom.
    /// </summary>
    private Vector3 Diverge(List<Collider> goAwayFrom)
    {
        if (goAwayFrom.Count == 0) { return Vector3.zero; }
        Vector3 result = Vector3.zero;
        foreach (Collider c in goAwayFrom)
        {
            result -= c.transform.position;
        }
        result.y = 0;
        return (m_Parent.transform.position - result);
    }

    /// <summary>
    /// Returns a vector that is the normalized average of the velocity vectors
    /// of all colliders in alignWith.
    /// </summary>
    private Vector3 Align(List<Collider> alignWith)
    {
        if (alignWith.Count == 0) { return Vector3.zero; }
        Vector3 result = Vector3.zero;
        foreach (Collider c in alignWith)
        {
            result += c.GetComponent<Rigidbody>().velocity;
        }
        result.y = 0;
        return (m_Parent.transform.position - result);
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

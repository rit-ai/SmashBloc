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
 * 
 * This class should have no public state or methods. Avoid conditional 
 * branches and jumps as much as possible.
 * **/
public class TwirlPhysics : MobilePhysics
{
    // **         //
    // * FIELDS * //
    //         ** //

    // Thresholds for unit convergence and separation
    private const float MAX_DISTANCE_FROM = 400f; // "sight range"
    private const float MIN_DISTANCE_FROM = 100f;
    // Height above ground at which float force is applied
    private const float MAX_FLOAT_THRESHOLD = 12f;
    // Distance from destination at which deceleration begins (squared)
    private const float DECELERATION_THRESHOLD_SQRD = 10000f;
    private const float UP_FORCE = 100f;
    // Default steering force strength
    private const float SPEED = 300f;
    // Adjusts the intensity of steering forces
    private const float GUIDANCE_FACTOR = 1f;
    private const float CONVERGE_FACTOR = 0.25f;
    private const float DIVERGE_FACTOR = 1f;
    // How many colliders to keep track of
    private const int COLLIDER_MEM = 50;

    private Collider[] convergeWith;
    private Collider[] divergeWith;
    private Vector3 converge;
    private Vector3 diverge;
    private Twirl parent;
    private Rigidbody body;
    private Rigidbody hoverBall;
    private Rigidbody bottomWeight;
    private int convergeCount;
    private int divergeCount;

    // **          //
    // * METHODS * //
    //          ** //

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
        Vector3 downForce = Vector3.down * bottomWeight.mass;
        bottomWeight.AddForce(downForce, ForceMode.Acceleration);

        // If the unit is too far from the floor, don't apply any force to the
        // hoverball
        if (Physics.Raycast(hoverBall.transform.position, Vector3.down, out hit, MAX_FLOAT_THRESHOLD, Toolbox.Terrain.ignoreAllButTerrain))
        {
            // Get the destination height
            Vector3 destination = Vector3.up * MAX_FLOAT_THRESHOLD * Mathf.Abs(Physics.gravity.y);
            // Adjust UP by the masses of the hoverball and parent rigidbody
            Vector3 desire = Vector3.up * (destination.y - body.velocity.y * UP_FORCE);
            desire -= hoverBall.velocity;
            // Cap the amount of force (to prevent strange launches)
            desire = Vector3.ClampMagnitude(desire, MAX_VECTOR_FORCE);
            // Apply force
            hoverBall.AddForce(desire, ForceMode.Acceleration);
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
        // Get the vector representing the desired trajectory. The Y component
        // of this vector will always be the target hover height.
        Vector3 desire = new Vector3(
                parent.Destination.x - parent.transform.position.x,
                MAX_FLOAT_THRESHOLD - parent.transform.position.y,
                parent.Destination.z - parent.transform.position.z
            );
        // Store the magnitude for later use
        float desireMagnitude = desire.sqrMagnitude;
        // Normalize and adjust the vector to use max speed by default
        desire = desire.normalized * SPEED * GUIDANCE_FACTOR;
        // Lower that speed depending on its distance from the destination
        desire *= Decelerate(desireMagnitude / DECELERATION_THRESHOLD_SQRD);
        // Steering = desire - velocity
        Vector3 velocity = body.velocity;
        velocity.y = 0;
        desire -= velocity;
        // Cap the amount of force (to prevent strange launches)
        desire = Vector3.ClampMagnitude(desire, MAX_VECTOR_FORCE);
        // Reduce the amount of force if not hovering
        // Add the force
        hoverBall.AddForce(desire, ForceMode.Acceleration);
    }

    /// <summary>
    /// Hoverballs will try to stay within a certain radius of each other, but 
    /// also outside a certain radius of each other, and also generally go in
    /// the same direction as others. They will try to avoid colliding with 
    /// units on the opposite team.
    /// </summary>
    /// <returns>A normalized Vector3 that accounts for both convergent, 
    /// divergent and alignment forces.</returns>
    private void Flock()
    {
        // Get all units that are within "sight" range.
        convergeCount = Physics.OverlapSphereNonAlloc(transform.position, MAX_DISTANCE_FROM, convergeWith, parent.ignoreAllButMobiles);
        divergeCount = Physics.OverlapSphereNonAlloc(transform.position, MIN_DISTANCE_FROM, divergeWith, parent.ignoreAllButMobiles);

        // Multiply the components of the convergent force by the components of
        // the divergent force and normalize the result.
        converge = Converge(convergeWith, convergeCount).normalized;
        diverge = Diverge(divergeWith, divergeCount).normalized;

        converge = Vector3.ClampMagnitude(converge * SPEED * CONVERGE_FACTOR, MAX_VECTOR_FORCE);
        diverge = Vector3.ClampMagnitude(diverge * SPEED * DIVERGE_FACTOR, MAX_VECTOR_FORCE);

        body.AddForce(converge, ForceMode.Acceleration);
        body.AddForce(diverge, ForceMode.Acceleration);
    }

    /// <summary>
    /// Returns a vector representing the direction of the general center of 
    /// the colliders in goToward, as well as those colliders' general 
    /// direction.
    /// </summary>
    private Vector3 Converge(Collider[] goToward, int count)
    {
        Vector3 result = Vector3.zero;
        for (int x = 0; x < count; x++)
        {
            result += goToward[x].transform.position;
            result += goToward[x].GetComponent<Rigidbody>().velocity;
        }
        result.y = 0;
        return (parent.transform.position - result);
    }

    /// <summary>
    /// Returns a  vector in the general direction away from the colliders in 
    /// goAwayFrom.
    /// </summary>
    private Vector3 Diverge(Collider[] goAwayFrom, int count)
    {
        Vector3 result = Vector3.zero;
        for (int x = 0; x < count; x++)
        {
            result -= goAwayFrom[x].transform.position;
        }
        result.y = 0;
        return (parent.transform.position - result);
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

    /// <summary>
    /// Initialize state.
    /// </summary>
    private void Start()
    {
        parent = GetComponent<Twirl>();
        body = parent.GetComponent<Rigidbody>();
        hoverBall = parent.hoverBall;
        bottomWeight = parent.bottomWeight;

        convergeWith = new Collider[COLLIDER_MEM];
        divergeWith = new Collider[COLLIDER_MEM];

        body.useGravity = true;
        bottomWeight.useGravity = true;
    }

}

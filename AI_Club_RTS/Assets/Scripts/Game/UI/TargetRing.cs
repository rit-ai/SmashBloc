using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This script controls the Target Ring, particularly its animations. The 
 * Target Ring is visible onscreen whenever a command is issued for units to 
 * travel to a new destination.
 * **/
public class TargetRing : MonoBehaviour {

    [Tooltip("The distance up and down that the ring hovers.")]
    public float hoverDistance;
    [HideInInspector] // FIXME I couldn't get this one working.
    [Tooltip("The temporal offset of the cones hover function. 0 to make it the" +
            " same as the ring, 1 to make it the opposite (0-1).")]
    public float coneHoverDelay;
    [Tooltip("The rate at which the cones spin around the center (0-2).")]
    public float spinRate;

    // We'd sink into the ground if not for this
    private const float ABOVE_GROUND_OFFSET = 2.25f;

    private Transform ring;
    private Transform conerig;
    private float hoverOffset;
    private bool reset; // have we changed our position?

    // Use this for initialization
    void Start () {
        ring = GetComponent<Transform>();
        conerig = transform.GetChild(0).GetComponent<Transform>();

        spinRate = Mathf.Clamp(spinRate, 0f, 2f);
        coneHoverDelay = Mathf.Clamp(coneHoverDelay, 0f, 1f);

        StartCoroutine(AnimateHover(ring, hoverDistance));
        StartCoroutine(AnimateHover(conerig, hoverDistance / 2f, coneHoverDelay));
        StartCoroutine(AnimateConeRotation(conerig));
    }

    /// <summary>
    /// Updates the position of the target ring and resets its animation.
    /// </summary>
    /// <param name="position">The new position to move to, likely with a 
    /// different xz than the current one.</param>
    public void UpdatePosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y + ABOVE_GROUND_OFFSET, position.z);
        reset = true;
    }

    /// <summary>
    /// Updates the position without changing our xz.
    /// </summary>
    private void UpdatePosition(Transform hoverer)
    {
        hoverer.transform.position = new Vector3(hoverer.transform.position.x, hoverer.transform.position.y + hoverOffset, hoverer.transform.position.z);
    }

    /// <summary>
    /// Causes the hoverer to float up and down.
    /// </summary>
    /// <param name="hoverer">The transform to update.</param>
    /// <param name="hover">The distance to hover.</param>
    /// <param name="time">A temporal offset to the animation cycle.</param>
    private IEnumerator AnimateHover(Transform hoverer, float hover, float time = 0f)
    {
        float temp; // for swapping
        float localTime = time;
        float max = -hover;
        float min = hover;

        while (true)
        {
            // If we've changed position, reset the animation
            if (reset)
            {
                max = -hover;
                min = hover;
                localTime = time;
                reset = false;
            }

            hoverOffset = Mathf.Lerp(min, max, localTime);
            UpdatePosition(hoverer);

            localTime += Time.deltaTime;

            // Swap min and max to reverse direction
            if (localTime > 1f)
            {
                temp = max;
                max = min;
                min = temp;
                localTime -= 1f;
            }

            yield return 0f;
        }

    }
    
    /// <summary>
    /// Causes the rig to rotate slowly about its Z axis.
    /// </summary>
    private IEnumerator AnimateConeRotation(Transform rig)
    {
        const float BOOST = 100f; // very small z values can be lost
        float z = 0f;


        while (true)
        {
            z = Time.deltaTime * spinRate * BOOST;
            rig.transform.Rotate(0f, 0f, z);

            yield return 0f;
        }
    }
}

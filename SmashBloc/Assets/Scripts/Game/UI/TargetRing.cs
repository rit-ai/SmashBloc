using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This script controls the Target Ring, which is visible onscreen whenever the 
 * ground is right-clicked (by default) by the Player.
 * **/
public class TargetRing : MonoBehaviour
{
    // **         //
    // * FIELDS * //
    //         ** //

    [Tooltip("The distance up and down that the ring hovers.")]
    public float hoverDistance;
    [Tooltip("The temporal offset of the cones hover function. 0 to make it the" +
            " same as the ring, 1 to make it the opposite (0 - 1).")]
    public float coneHoverDelay;
    [Tooltip("The rate at which the cones spin around the center (0 - 2).")]
    public float spinRate;
    [Tooltip("Should the ring spin clockwise?")]
    public bool spinClockwise;

    // We'd sink into the ground if not for this
    private const float ABOVE_GROUND_OFFSET = 2.25f;
    
    private Transform ring;
    private Transform conerig;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Takes the public fields and implements them, then starts the rotation 
    /// of the transform.
    /// </summary>
    private void Start () {
        ring = transform;
        conerig = transform.GetChild(0).GetComponent<Transform>();

        spinRate = Mathf.Clamp(spinRate, 0, 2f);
        if (spinClockwise) { spinRate = -spinRate; }
        coneHoverDelay = Mathf.Clamp(coneHoverDelay, 0f, 1f);

        StartCoroutine(Utils.AnimateHover(ring, hoverDistance));
        StartCoroutine(Utils.AnimateHover(conerig, hoverDistance / 2f, coneHoverDelay));
    }

    /// <summary>
    /// Rotate about the Z axis.
    /// </summary>
    private void Update()
    {
        conerig.transform.Rotate(0f, 0f, Time.deltaTime * spinRate * 100f);
    }
    
}

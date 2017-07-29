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

    // Use this for initialization
    private void Start () {
        ring = GetComponent<Transform>();
        conerig = transform.GetChild(0).GetComponent<Transform>();

        spinRate = Mathf.Clamp(spinRate, 0f, 2f);
        coneHoverDelay = Mathf.Clamp(coneHoverDelay, 0f, 1f);

        StartCoroutine(Utils.AnimateHover(ring, hoverDistance));
        StartCoroutine(Utils.AnimateHover(conerig, hoverDistance / 2f, coneHoverDelay));
    }

    private void Update()
    {
        conerig.transform.Rotate(0f, 0f, Time.deltaTime * spinRate * 100f);
    }
    
}

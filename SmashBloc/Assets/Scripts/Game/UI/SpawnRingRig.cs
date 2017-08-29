using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This script controls the ring that hovers around City units, consequently 
 * moving the location where Mobiles spawn.
 * **/
public class SpawnRingRig : MonoBehaviour
{
    // **         //
    // * FIELDS * //
    //         ** //

    public Transform outerRing;
    public Transform innerRing;

    [Tooltip("The height of the hover, both upwards and downwards.")]
    public float hoverHeight;
    [Tooltip("The rate at which the spawn ring rotates around a City.")]
    public float spinRate;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Updates the color of the rings, usually for when a city is captured.
    /// </summary>
    public void UpdateColor(Color color)
    {
        outerRing.GetComponent<MeshRenderer>().material.color = color;
        innerRing.GetComponent<MeshRenderer>().material.color = color;
    }

    public void Init(Color color)
    {
        StartCoroutine(Utils.AnimateHover(outerRing, hoverHeight));
        StartCoroutine(Utils.AnimateHover(innerRing, hoverHeight / 2f, 0.5f));
        UpdateColor(color);
    }
	
	// Rotates about the Y axis
	void Update () {
        transform.Rotate(0f, spinRate * Time.deltaTime, 0f);
	}
}

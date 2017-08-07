using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRingRig : MonoBehaviour {

    public Transform outerRing;
    public Transform innerRing;

    [Tooltip("The height of the hover, both upwards and downwards.")]
    public float hoverHeight;
    [Tooltip("The rate at which the spawn ring rotates around a City.")]
    public float spinRate;

    /// <summary>
    /// Updates the color of the rings, usually for when a city is captured.
    /// </summary>
    public void UpdateColor(Color color)
    {
        outerRing.GetComponent<MeshRenderer>().material.color = color;
        innerRing.GetComponent<MeshRenderer>().material.color = color;
    }

	void Start () {
        StartCoroutine(Utils.AnimateHover(outerRing, hoverHeight));
        StartCoroutine(Utils.AnimateHover(innerRing, hoverHeight / 2f, 0.5f));

        // A complex way of saying we want to be the same color as the parent.
        outerRing.GetComponent<MeshRenderer>().material.color = GetComponentInParent<City>().gameObject.GetComponent<MeshRenderer>().material.color;
        innerRing.GetComponent<MeshRenderer>().material.color = GetComponentInParent<City>().gameObject.GetComponent<MeshRenderer>().material.color;
    }
	
	// Rotates about the Y axis
	void Update () {
        transform.Rotate(0f, spinRate * Time.deltaTime, 0f);
	}
}

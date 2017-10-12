using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * @author Paul Galatic
 * 
 * Prevents a game object from changing its rotation. 
 * **/
public class NoRotate : MonoBehaviour {

    Quaternion initialRotation;

	// Use this for initialization
	void Start () {
        initialRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = initialRotation;
	}
}

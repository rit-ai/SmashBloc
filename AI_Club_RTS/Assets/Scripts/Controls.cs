using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
	// Author: Ben Fairlamb
	// Purpose: Controls
	// Limitations: Meh

	// Fields
	Camera camera;
	private PathfindingTester unit;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera> ();

		// Find unit and get PathfindingTester script to access
		unit = GameObject.Find ("Unit").GetComponent<PathfindingTester> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Get ray from camera through mouse
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		//Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

		// Cast ray and store hit into RacastHit Object
		RaycastHit hit;
		Physics.Raycast (ray, out hit);

		// Pass transform of hit to PathfindingTester to set as destination
		if (hit.transform != null) {
			unit.NewDestination (hit.transform);
		}

		Debug.Log (hit.transform.ToString());
	}
}

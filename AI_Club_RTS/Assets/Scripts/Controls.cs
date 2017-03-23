using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
	// Author: Ben Fairlamb
	// Purpose: Controls
	// Limitations: Meh

	// Fields
	Camera camera;
	public PathfindingTester unit;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		//Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
		RaycastHit hit;
		Physics.Raycast (ray, out hit);
		unit.Destination = hit.transform;
		Debug.Log (hit.transform.ToString());
	}
}

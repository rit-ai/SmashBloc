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
	private GameObject unitObject;
	private RaycastHit hit;
	private bool active = false;

	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera> ();

		// Find unit and get PathfindingTester script to access
		/*unitObject = GameObject.FindGameObjectWithTag("Unit");
		if (unit != null) {
			Debug.Log ("Unit found");
			unit = unitObject.GetComponent<PathfindingTester> ();
		} else {
			Debug.Log ("Unit not found");
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			// Get ray from camera through mouse
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			//Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

			// Cast ray and store hit into RacastHit Object
			Physics.Raycast (ray, out hit);

			// Pass transform of hit to PathfindingTester to set as destination
			if (hit.transform != null) {
				unit.NewDestination (hit.transform);
				Debug.Log (hit.transform.ToString ());
			}
		}
	}

	/// <summary>
	/// Hail-Mary attempt of getting a reference between this script and the individual Unit Scripts
	/// </summary>
	/// <param name="unit">Unit to attach to Camera</param>
	/*public void AttachUnit(GameObject unit) {
		unitObject = unit;
		if (unit != null) {
			Debug.Log ("Unit found");
			unit = unitObject.GetComponent<PathfindingTester> ();
		} else {
			Debug.Log ("Unit not found");
		}
	}*/

}
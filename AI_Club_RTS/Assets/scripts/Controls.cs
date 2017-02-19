using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
	// Author: Ben Fairlamb
	// Purpose: Controls
	// Limitations: Meh

	// Fields
	Camera camera;
	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = camera.ScreenPointToRay(new Vector3(200, 200, 0));
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
	}
}

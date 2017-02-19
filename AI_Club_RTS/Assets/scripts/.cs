using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
	// Author: Ben Fairlamb
	// Purpose: Player Unit interaction through Camera
	// Limitations: Meh

	// Fields
	Camera camera;

	void Start() {
		camera = GetComponent<Camera>();
	}

	void Update() {
		Ray ray = camera.ScreenPointToRay(new Vector3(200, 200, 0));
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingTester : MonoBehaviour {
	// Author: Ben Fairlamb
	// Purporse: Pathfinding Test
	// Limnitations: Meh

	// Fields
	private GameObject[] points;
	private UnityEngine.AI.NavMeshAgent agent;
	private Camera camera;
	private Controls controls;

	// Properties
	/// <summary>
	/// Gets the points.
	/// </summary>
	/// <value>The points.</value>
	public GameObject[] Points {
		get { return points; }
	}

	// Methods
	// Use this for initialization
	void Start () {
		// Load points, the navAgent, and set a new destination
		//points = GameObject.FindGameObjectsWithTag ("Point");
		//agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		//NewDestination ();

		// Hail-Mary Attempt to get connection between Unit and Controls Scripts
		/*camera = FindObjectOfType<Camera> ();
		if (Camera != null) {
			controls = camera.GetComponent<Controls> ();
			controls.AttachUnit (this);
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		// Check if arrived and give new destination if nessesary
		/*if (agent.remainingDistance < .1) {
			NewDestination ();
		}*/

	}

	/// <summary>
	/// Sets a New Destination.
	/// </summary>
	public void NewDestination(Transform destination) {
		agent.destination = destination.position;
	}
}

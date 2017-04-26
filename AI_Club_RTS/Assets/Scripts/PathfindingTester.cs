using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingTester : MonoBehaviour {
	// Author: Ben Fairlamb
	// Purporse: Pathfinding Test
	// Limnitations: Meh

	// Fields
	private Transform[] points;
	private UnityEngine.AI.NavMeshAgent agent;
	private Camera camera;
	private Controls controls;
	Terrain thisTerrain;

	// Properties
	/// <summary>
	/// Gets the points.
	/// </summary>
	/// <value>The points.</value>
	public Transform[] Points {
		get { return points; }
	}

	// Methods
	// Use this for initialization
	void Start () {
		// Load points, the navAgent, and set a new destination
		points = GameObject.Find("Points").GetComponentsInChildren<Transform>();
		thisTerrain = FindObjectOfType<Terrain> ();
		agent = GetComponent<NavMeshAgent>();
		agent.destination = points [1].position;

		Debug.Log (agent.destination);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (1)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100)) {
				agent.destination = hit.point;
			}
		
		}
	}

	/// <summary>
	/// Sets a New Destination.
	/// </summary>
	public void NewDestination(Transform destination) {
		agent.destination = destination.position;
	}
}

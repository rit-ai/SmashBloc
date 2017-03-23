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
	private Transform destination;

	// Properties
	/// <summary>
	/// Gets the points.
	/// </summary>
	/// <value>The points.</value>
	public GameObject[] Points {
		get { return points; }
	}

	public Transform Destination
	{
		get { return destination; }
		set { destination = value; }
	}

	// Methods
	// Use this for initialization
	void Start () {
		// Load points, the navAgent, and set a new destination
		points = GameObject.FindGameObjectsWithTag ("Point");
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		//NewDestination ();
	}
	
	// Update is called once per frame
	void Update () {
		// Check if arrived and give new destination if nessesary
		/*if (agent.remainingDistance < .1) {
			NewDestination ();
		}*/

		// Have object follow mouse
		if (destination != null) {
			agent.destination = destination.position;
		}
	}

	/// <summary>
	/// Sets a New Destination.
	/// </summary>
	public void NewDestination() {
		//agent.destination = points [Random.Range (0, points.Length)].transform.position;
	}
}

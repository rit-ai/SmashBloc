using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour {
    // Author: Ben Fairlamb
    // Purpose: City functionality
    // Limitations: Meh

    // Constants
    private int DEFAULT_INCOME = 1;

    // Fields
    private string team;
    private int income;

	// Properties
	/// <summary>
	/// Gets the Team that currently owns the city.
	/// </summary>
	/// <value>The Team that owns the city.</value>
	public string Team {
		get { return team; }
	}

	/// <summary>
	/// Gets the Income the city generates per second.
	/// </summary>
	/// <value>The Income the city generates per second.</value>
	public int Income {
		get { return income; }
	}

	// Use this for initialization
	void Start () {
        income = DEFAULT_INCOME;
    }

    // Update is called once per frame
    void Update () {
	}
}

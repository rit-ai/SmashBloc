using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Ben Fairlamb
 * @author Paul Galatic
 * 
 * Class designed to handle City-specific functionality and state. Like with 
 * other game objects, menus and UI elements should be handled by observers.
 * **/
public class City : MonoBehaviour {
    // Private constants
    private int DEFAULT_INCOME = 1;

    // Private fields
    // private string team;
    private int income;

	// Properties
	/// <summary>
	/// Gets the Team that currently owns the city.
	/// </summary>
	/// <value>The Team that owns the city.</value>
	//public string Team {
		//get { return team; }
	//}

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

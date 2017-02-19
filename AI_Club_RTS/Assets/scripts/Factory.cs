using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {
	// Author: Ben Fairlamb
	// Purpose: Factory functionality
	// Limitations: Meh

	// Fields
	private int health;
	private string team;

	// Properties
	/// <summary>
	/// Gets the Health of the factory.
	/// </summary>
	/// <value>The Health of the factory.</value>
	public int Health {
		get { return health; }
	}

	/// <summary>
	/// Gets the Team that currently owns the factory.
	/// </summary>
	/// <value>The Team that currently owns the factory.</value>
	public string Team {
		get { return team; }
	}

	// Constructor
	public Factory()
	{
		health = 0;
		team = "nuetral";
	}

	// Methods
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Builds an infantry.
	/// </summary>
	public void BuildInfantry()
	{
		Infantry newInf = new Infantry (team);
		Instantiate (newInf);
	}

	/// <summary>
	/// Builds a tank.
	/// </summary>
	public void BuildTank()
	{
		Tank newTank = new Tank (team);
		Instantiate (newTank);
	}

	/// <summary>
	/// Builds a bazooka.
	/// </summary>
	public void BuildBazooka()
	{
		Bazooka newZook = new Bazooka (team);
		Instantiate (newZook);
	}

	/// <summary>
	/// Builds a recon.
	/// </summary>
	public void BuildRecon()
	{
		Recon newRec = new Recon (team);
		Instantiate (newRec);
	}

	/// <summary>
	/// Builds an artillery.
	/// </summary>
	public void BuildArtillery()
	{
		Artillery newArt = new Artillery (team);
		Instantiate (newArt);
	}
}

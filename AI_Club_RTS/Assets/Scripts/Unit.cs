using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {

	// Author: Ben Fairlamb
	// Purpose: Base class for all Units
	// Limitations: Meh

	// fields
	protected string team;
	protected int maxHealth;
	protected int health;
	protected string unitType;
	protected string dmgType;
	protected int dmg;
	protected int range;
	//protected int cost; // Might be usefull later

	// Properties
	/// <summary>
	/// Gets the Team of the unit.
	/// </summary>
	/// <value>The Team of the unit.</value>
	public string Team {
		get { return team; }
	}

	/// <summary>
	/// Gets the unit's Maximum Health.
	/// </summary>
	/// <value>The unit's maximum health.</value>
	public int MaxHealth {
		get { return maxHealth; }
	}

	/// <summary>
	/// Gets the unit's current Health.
	/// </summary>
	/// <value>The unit's current Health.</value>
	public int Health {
		get { return health; }
	}

	/// <summary>
	/// Gets the type of the unit.
	/// </summary>
	/// <value>The type of the unit.</value>
	public string UnitType {
		get { return unitType; }
	}

	/// <summary>
	/// Gets the unit's Type of Damage. (Possible use for Rock Paper Scissors effect: Temp)
	/// </summary>
	/// <value>The unit's Type of Damage.</value>
	public string DmgType {
		get { return dmgType; }
	}

	/// <summary>
	/// Gets the Damage dealt by the unit.
	/// </summary>
	/// <value>The Damage dealt by the unit.</value>
	public int Dmg {
		get { return dmg; }
	}

	/// <summary>
	/// Gets the unit's Range.
	/// </summary>
	/// <value>The unit's Range.</value>
	public int Range {
		get { return range; }
	}

	/*/// <summary>
	/// Gets the Cost of the unit.
	/// </summary>
	/// <value>The unit's cost.</value>
	public int Cost {
		get { return cost; }
	}*/

	// Constructor
	/// <summary>
	/// Initializes a new instance of the <see cref="Unit"/> class.
	/// </summary>
	/// <param name="maxhealth">Unit's Maximum Health.</param>
	/// <param name="dmgType">Unit's Damage Type.</param>
	/// <param name="dmg">Unit's Damage.</param>
	/// <param name="range">Unit's Range.</param>
	/// <param name="cost">Unit's Cost.</param>
	public Unit(string team, int maxHealth, string unitType, string dmgType, int dmg, int range/*, int cost*/) {
		this.team = team;
		this.maxHealth = maxHealth;
		health = maxHealth;
		this.unitType = unitType;
		this.dmgType = dmgType;
		this.dmg = dmg;
		this.range = range;
		//this.cost = cost;
	}


	// Methods
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	/// Attack the specified target.
	/// </summary>
	/// <param name="target">Target to attack.</param>
	public abstract void Attack(Unit target);

	/// <summary>
	/// Take specified damage.
	/// </summary>
	/// <param name="dmg">Damage to Take.</param>
	public abstract void TakeDmg(int dmg);

	/// <summary>
	/// Kill this instance.
	/// </summary>
	public abstract void Kill();
}

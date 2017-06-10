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
	protected ArmorType armorType;
	protected DamageType dmgType;
	protected int dmg;
	protected int range;
	protected int cost;

    protected Rigidbody body;
    protected Collider collision;

	// Properties
	/// <summary>
	/// Gets the Team of the unit.
	/// </summary>
	public string Team {
		get { return team; }
	}

	/// <summary>
	/// Gets the unit's Maximum Health.
	/// </summary>
	public int MaxHealth {
		get { return maxHealth; }
	}

	/// <summary>
	/// Gets the unit's current Health.
	/// </summary>
	public int Health {
		get { return health; }
	}

	/// <summary>
	/// Gets the type of the unit.
	/// </summary>
	public ArmorType ArmorType {
		get { return armorType; }
	}

	/// <summary>
	/// Gets the unit's Type of Damage. (Possible use for Rock Paper Scissors effect: Temp)
	/// </summary>
	public DamageType DmgType {
		get { return dmgType; }
	}

	/// <summary>
	/// Gets the Damage dealt by the unit.
	/// </summary>
	public int Dmg {
		get { return dmg; }
	}

	/// <summary>
	/// Gets the unit's Range.
	/// </summary>
	public int Range {
		get { return range; }
	}

	/// <summary>
	/// Gets the Cost of the unit.
	/// </summary>
	public int Cost {
		get { return cost; }
	}

	// Constructor
	/// <summary>
	/// Initializes a new instance of the <see cref="Unit"/> class.
	/// </summary>
	/// <param name="maxhealth">Unit's Maximum Health.</param>
	/// <param name="dmgType">Unit's Damage Type.</param>
	/// <param name="dmg">Unit's Damage.</param>
	/// <param name="range">Unit's Range.</param>
	/// <param name="cost">Unit's Cost.</param>
	public Unit(string team, int maxHealth, ArmorType armorType, DamageType dmgType, int dmg, int range, int cost) {
		this.team = team;
		this.maxHealth = maxHealth;
		this.armorType = armorType;
		this.dmgType = dmgType;
		this.dmg = dmg;
		this.range = range;
		this.cost = cost;

        health = maxHealth;
	}

    public void ToggleHighlight()
    {

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

public enum ArmorType
{
    H_ARMOR, M_ARMOR, L_ARMOR
}

public enum DamageType
{
    EXPLOSIVE, BULLET
}
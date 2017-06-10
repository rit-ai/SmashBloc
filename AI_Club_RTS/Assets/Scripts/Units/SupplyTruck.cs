using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyTruck : Unit {
    // Author: Ben Fairlamb
    // Purpose: Supply Truck Class

    private const int MAXHEALTH = 100;
    private const ArmorType ARMOR_TYPE = ArmorType.L_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;
    private const int DAMAGE = 20;
    private const int RANGE = 50;
    private const int COST = 200;

    // Fields
    private const int supplyCapacity = 100;
	private int supplies;

	// Properties
	/// <summary>
	/// Gets the supply capacity of the truck.
	/// </summary>
	/// <value>The supply capacity of the truck.</value>
	public int SupplyCapacity {
		get { return supplyCapacity; }
	}

	/// <summary>
	/// Gets the amount of supplies in the truck.
	/// </summary>
	/// <value>The amount of supplies in the truck.</value>
	public int Supplies {
		get { return supplies; }
	}

	// Constructor
	/// <summary>
	/// Initializes a new instance of the Supply Truck class.
	/// </summary>
	/// <param name="team">Team of the truck.</param>
	public SupplyTruck(string team)
        : base(team, MAXHEALTH, ARMOR_TYPE, DMG_TYPE, DAMAGE, RANGE, COST)
    {
        supplies = supplyCapacity;
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
	public override void Attack(Unit target)
	{
		// Mini Pew-Pew
	}

	/// <summary>
	/// Take specified damage.
	/// </summary>
	/// <param name="dmg">Damage to Take.</param>
	/// <param name="amount">Amount.</param>
	public override void TakeDmg(int amount)
	{
	}

	/// <summary>
	/// Kill this instance.
	/// </summary>
	public override void Kill()
	{
	}
}

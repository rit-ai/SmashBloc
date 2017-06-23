using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyTruck : Unit {
    // Author: Ben Fairlamb
    // Purpose: Supply Truck Class

    // NAME -- for external reference purposes
    public const string IDENTITY = "SupplyTruck";

    // CONSTANTS -- intimately related to unit design
    private const ArmorType ARMOR_TYPE = ArmorType.L_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;
    private const int SUPPLY_CAPACITY = 100;

    // Default values
    private const string NAME = "SupplyTruck";
    private const int MAXHEALTH = 100;
    private const int DAMAGE = 20;
    private const int RANGE = 50;
    private const int COST = 200;

    // Fields
	private int supplies;

    // Methods
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        // Handle constants
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        // Handle default values
        maxHealth = MAXHEALTH;
        dmg = DAMAGE;
        range = RANGE;
        cost = COST;
        // Handle private fields
        supplies = SUPPLY_CAPACITY;
    }

    /// <summary>
    /// Sets a new destination, which the unit will attempt to navigate toward.
    /// </summary>
    /// <param name="newDest"></param>
    public override void SetDestination(Vector3 newDest)
    {
        ai.OnDestChanged();
    }

    /// <summary>
    /// Returns identity of the unit, for disambiguation purposes.
    /// </summary>
    public override string Identity()
    {
        return IDENTITY;
    }

    // Properties
    /// <summary>
    /// Gets the supply capacity of the truck.
    /// </summary>
    /// <value>The supply capacity of the truck.</value>
    public int SupplyCapacity {
		get { return SUPPLY_CAPACITY; }
	}

	/// <summary>
	/// Gets the amount of supplies in the truck.
	/// </summary>
	/// <value>The amount of supplies in the truck.</value>
	public int Supplies {
		get { return supplies; }
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

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : Unit {
    // Author: Ben Fairlamb
    // Author: Paul Galatic
    // Purpose: Artillery unit

    // NAME -- for external reference purposes
    public const string IDENTITY = "ARTILLERY";

    // CONSTANTS -- intimately related to unit design
    private static ArmorType ARMOR_TYPE = ArmorType.H_ARMOR;
    private static DamageType DMG_TYPE = DamageType.EXPLOSIVE;

    // Default values
    private const float MAXHEALTH = 50f;
    private const float DAMAGE = 100f;
    private const float RANGE = 200f;
    private const int COST = 400;

	// Methods
	// Use this for initialization
	public override void Start () {
        base.Start();
        // Handle components
        // physics = new ArtilleryPhysics();
        // ai = new ArtilleryAI();
        // Handle constants
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        // Handle default values
        maxHealth = MAXHEALTH;
        damage = DAMAGE;
        attackRange = RANGE;
        cost = COST;
	}

    /// <summary>
    /// Sets a new destination, which the unit will attempt to navigate toward.
    /// </summary>
    /// <param name="newDest"></param>
    public override void SetDestination(Vector3 newDest)
    {
        
    }

    /// <summary>
    /// Returns identity of the unit, for disambiguation purposes.
    /// </summary>
    public override string Identity()
    {
        return IDENTITY;
    }

    /// <summary>
    /// Attack the specified target.
    /// </summary>
    /// <param name="target">Target to attack.</param>
    public override void Attack(Unit target)
	{
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
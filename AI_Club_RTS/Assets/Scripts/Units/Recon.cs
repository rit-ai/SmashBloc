using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recon : Unit {
    // Author: Ben Fairlamb
    // Purpose: Recon unit

    // NAME -- for external reference purposes
    public const string IDENTITY = "RECON";

    // CONSTANTS -- intimately related to unit design
    private const ArmorType ARMOR_TYPE = ArmorType.L_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;

    // Default values
    private const int MAXHEALTH = 100;
    private const int DAMAGE = 100;
    private const int RANGE = 25;
    private const int COST = 200;

	// Methods
	// Use this for initialization
	public override void Start () {
        base.Start();
        // Handle constants
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        // Handle default values
        maxHealth = MAXHEALTH;
        dmg = DAMAGE;
        range = RANGE;
        cost = COST;
    }

    /// <summary>
    /// Sets a new destination, which the unit will attempt to navigate toward.
    /// </summary>
    /// <param name="newDest"></param>
    public override void SetDestination(Vector3 newDest)
    {
        ai.SetDestChanged();
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

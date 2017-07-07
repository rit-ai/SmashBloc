using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit {
    // Author: Ben Fairlamb
    // Purpose: Tank unit

    public const string IDENTITY = "Tank";
    public const int COST = 500;

    // CONSTANTS -- intimately related to unit design
    private const ArmorType ARMOR_TYPE = ArmorType.H_ARMOR;
    private const DamageType DMG_TYPE = DamageType.EXPLOSIVE;

    // Default values
    private const string NAME = "Tank";
    private const int MAXHEALTH = 200;
    private const int DAMAGE = 100;
    private const int RANGE = 100;

    // Methods
    // Use this for initialization
    protected override void Start () {
        base.Start();
        // Handle constants
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        // Handle default values
        unitName = NAME;
        maxHealth = MAXHEALTH;
        damage = DAMAGE;
        attackRange = RANGE;
        // Handle fields
        health = MAXHEALTH;
    }

    /// <summary>
    /// Returns identity of the unit, for disambiguation purposes.
    /// </summary>
    public override string Identity()
    {
        return IDENTITY;
    }

    /// <summary>
    /// The cost of a Tank, in gold.
    /// </summary>
    /// <returns></returns>
    public override int Cost()
    {
        return COST;
    }

    protected override void OnCollisionEnter(Collision collision)
    {

    }

    protected override IEnumerator DeathAnimation()
    {
        yield return null;
    }
}

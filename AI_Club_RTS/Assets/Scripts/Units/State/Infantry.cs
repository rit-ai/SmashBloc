using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Unit {
    // Author: Ben Fairlamb
    // Author: Paul Galatic
    // Purpose: Infantry unit

    // Public constants
    public const string IDENTITY = "Infantry";

    // Public fields
    public Rigidbody m_Hoverball;
    public Rigidbody m_BottomWeight;

    // Private constants
    private InfantryPhysics physics;
    private const ArmorType ARMOR_TYPE = ArmorType.M_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;
    // Default values
    private const int MAXHEALTH = 100;
    private const int DAMAGE = 10;
    private const int RANGE = 25;

    // Methods
    // Use this for initialization
    public override void Start () {
        // Handle components
        physics = new InfantryPhysics(this);
        ai = gameObject.AddComponent<BasicInfantryAI>();
        ai.Body = this;
        // Handle default values
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        // team = "NULL";
        maxHealth = MAXHEALTH;
        damage = DAMAGE;
        attackRange = RANGE;
        // Handle fields
        health = Random.Range(10f, 90f); //FIXME
        base.Start();
    }

    /// <summary>
    /// Handles general physics properties of units through the physics 
    /// component.
    /// </summary>
    public void FixedUpdate()
    {
        physics.ComponentUpdate();
    }

    /// <summary>
    /// Sets a new destination, which the unit will attempt to navigate toward.
    /// </summary>
    /// <param name="newDest"></param>
    public override void SetDestination(Vector3 newDest)
    {
        destination = newDest;

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

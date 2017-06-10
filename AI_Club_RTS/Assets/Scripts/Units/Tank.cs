using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit {
	// Author: Ben Fairlamb
	// Purpose: Tank unit

    private const int MAXHEALTH = 200;
    private const ArmorType ARMOR_TYPE = ArmorType.H_ARMOR;
    private const DamageType DMG_TYPE = DamageType.EXPLOSIVE;
    private const int DAMAGE = 100;
    private const int RANGE = 100;
    private const int COST = 500;

	// Constructor
	/// <summary>
	/// Initializes a new instance of the Tank class.
	/// </summary>
	public Tank(string team)
		: base(team, MAXHEALTH, ARMOR_TYPE, DMG_TYPE, DAMAGE, RANGE, COST)
	{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bazooka : Unit {
    // Author: Ben Fairlamb
    // Purpose: Bazooka unit

    private const int MAXHEALTH = 100;
    private const ArmorType ARMOR_TYPE = ArmorType.M_ARMOR;
    private const DamageType DMG_TYPE = DamageType.EXPLOSIVE;
    private const int DAMAGE = 100;
    private const int RANGE = 100;
    private const int COST = 300;

    // Constructor
    public Bazooka(string team)
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

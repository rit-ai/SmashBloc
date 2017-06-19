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
    public Rigidbody m_HoverPull;

    // Private constants
    private const float HOVER_QUOTIENT_MODIFIER = 100f;
    private const ArmorType ARMOR_TYPE = ArmorType.M_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;
    // Default values
    private const int MAXHEALTH = 100;
    private const int DAMAGE = 10;
    private const int RANGE = 25;


    // Methods
    // Use this for initialization
    void Start () {
        base.Init();
        // Handle constants
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        // Handle default values
        // team = "NULL";
        unitName = IDENTITY;
        maxHealth = MAXHEALTH;
        dmg = DAMAGE;
        range = RANGE;
        // Handle fields
        health = Random.Range(10f, 90f); //FIXME
    }

    /// <summary>
    /// Handles general physics properties of units.
    /// </summary>S
    public void FixedUpdate()
    {
        // Units will hover based on their current distance from the floor.
        // The farther a unit is from the floor, the less upward force is applied.
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity);
        if (!(hit.normal.magnitude == 0))
        {
            Vector3 hoverQuotient = Vector3.up * m_HoverPull.mass * Mathf.Abs(Physics.gravity.y);
            hoverQuotient = hoverQuotient / (hit.normal.magnitude * HOVER_QUOTIENT_MODIFIER);
            hoverQuotient.Scale(hit.normal);
            m_HoverPull.AddForce(hoverQuotient, ForceMode.Acceleration);
        }

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

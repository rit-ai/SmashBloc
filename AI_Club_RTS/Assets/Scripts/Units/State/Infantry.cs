using System;
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
    private const float ASCENSION_HEIGHT = 1000f;
    // Default values
    private const int MAXHEALTH = 100;
    private const int DAMAGE = 10;
    private const int RANGE = 25;

    // Methods
    // Use this for initialization
    protected override void Start () {
        // Handle components
        physics = new InfantryPhysics(this);
        ai = gameObject.AddComponent<UnitAI_Template>();
        ai.Body = this;
        // Handle default values
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        // team = "NULL";
        maxHealth = MAXHEALTH;
        health = MAXHEALTH;
        damage = DAMAGE;
        attackRange = RANGE;
        // Handle fields
        TakeDamage(UnityEngine.Random.Range(10f, 50f));
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
    /// Returns identity of the unit, for disambiguation purposes.
    /// </summary>
    public override string Identity()
    {
        return IDENTITY;
    }

    /// <summary>
    /// What to do when the unit collides with another unit that's not on the 
    /// same team.
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null && !(unit.Team.Equals(team)))
        {
            TakeDamage(UnityEngine.Random.Range(10f, 20f));
        }
        City city = collision.gameObject.GetComponent<City>();
        if (city != null && !(city.Team.Equals(team)))
        {
            TakeDamage(UnityEngine.Random.Range(10f, 20f));
        }
    }

    /// <summary>
    /// In this animation, the Infantry unit sails into the air before being
    /// destroyed. Particle effects are TODO.
    /// </summary>
    protected override IEnumerator DeathAnimation()
    {
        float y = transform.position.y;
        float dest = transform.position.y + ASCENSION_HEIGHT;
        for (float x = y; x < dest; x++)
        {
            newPos = transform.position;
            newPos.y += 10;
            transform.position = newPos;
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);

        yield return null;
    }
}

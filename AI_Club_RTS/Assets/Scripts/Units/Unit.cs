﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * @author Ben Fairlamb
 * @author Paul Galatic
 * 
 * This abstract class is designed to organize each child Unit. Logic that 
 * should be handled in Unit-type classes include:
 *  * State tracking
 *  * Decision making (AI)
 *  * Behavior execution
 *  * Basic transformation (particle effects, highlighting)
 * Any other UI elements should be handled by Observers.
 * **/
public abstract class Unit : MonoBehaviour {
    
    // public component fields
    public Canvas m_Canvas;
    public UI_Manager m_UI_Manager;

    // protected fields related to unit management
    protected List<Observer> observers;

    // protected fields intended to be changed for balancing or by gameplay
    protected string team;
    protected string name;
    protected string customName; // user-assigned names
    protected float maxHealth;
    protected float health;
    protected float dmg;
    protected float range;
    protected int cost;

    // protected fields related to fundamentals of unit type
    protected ArmorType armorType;
    protected DamageType dmgType;

    // protected fields related to physics
    protected Rigidbody body;
    protected Collider collision;

    /// <summary>
    /// Sets up Observers and other state common between Units.
    /// </summary>
    public void Init()
    {
        observers = new List<Observer>();
        observers.Add(new MenuObserver(m_UI_Manager));
    }

    public void NotifyObservers(string data)
    {
        foreach (Observer o in observers){
            o.OnNotify(this, data);
        }
    }

    /// <summary>
    /// Highlights the unit.
    /// </summary>
    public void Highlight()
    {
        m_Canvas.enabled = true;
    }

    /// <summary>
    /// Removes highlight from the unit.
    /// </summary>
    public void RemoveHighlight()
    {
        m_Canvas.enabled = false;
    }

    /// <summary>
    /// Logic handler for when the unit is individually selected, including
    /// notifying proper menu observers.
    /// </summary>
    public void SoloSelected()
    {
        NotifyObservers(MenuObserver.INVOKE_UNIT_DATA);
    }

    /// <summary>
    /// Attack the specified target.
    /// </summary>
    /// <param name="target">Target to attack.</param>
    public abstract void Attack(Unit target);

    /// <summary>
    /// Take specified damage.
    /// </summary>
    /// <param name="dmg">Damage to Take.</param>
    public abstract void TakeDmg(int dmg);

    /// <summary>
    /// Kill this instance.
    /// </summary>
    public abstract void Kill();

    // Properties
    /// <summary>
    /// Gets the Team of the unit.
    /// </summary>
    public string Team
    {
        get { return team; }
    }

    /// <summary>
    /// Gets the name of the unit.
    /// </summary>
    public string Name
    {
        get {
            if (!(customName == null))
            {
                return customName;
            }
            return name;
        }
    }

    /// <summary>
    /// Sets a custom name for this unit.
    /// </summary>
    public void setCustomName(string newName)
    {
        customName = newName;
    }

    /// <summary>
    /// Gets the type of the unit.
    /// </summary>
    public ArmorType ArmorType
    {
        get { return armorType; }
    }

    /// <summary>
    /// Gets the unit's Type of Damage. (Possible use for Rock Paper Scissors effect: Temp)
    /// </summary>
    public DamageType DmgType
    {
        get { return dmgType; }
    }

    /// <summary>
    /// Gets the unit's Maximum Health.
    /// </summary>
    public float MaxHealth
    {
        get { return maxHealth; }
    }

    /// <summary>
    /// Gets the unit's current Health.
    /// </summary>
    public float Health
    {
        get { return health; }
    }

    /// <summary>
    /// Gets the Damage dealt by the unit.
    /// </summary>
    public float Damage
    {
        get { return dmg; }
    }

    /// <summary>
    /// Gets the unit's Range.
    /// </summary>
    public float Range
    {
        get { return range; }
    }

    /// <summary>
    /// Gets the Cost of the unit.
    /// </summary>
    public int Cost
    {
        get { return cost; }
    }

}

/// <summary>
/// Type of armor. Armor type affects unit speed and damage resistance.
/// </summary>
public enum ArmorType
{
    H_ARMOR, M_ARMOR, L_ARMOR
}

/// <summary>
/// Type of damage. Explosive damage triggers knockback effects.
/// </summary>
public enum DamageType
{
    EXPLOSIVE, BULLET
}
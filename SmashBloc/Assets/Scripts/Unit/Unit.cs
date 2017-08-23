using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * @author Ben Fairlamb
 * @author Paul Galatic
 * 
 * This abstract class is designed to organize each child Unit. Logic that 
 * should be handled in Unit-type classes include:
 *  * State tracking
 *  * Decision making (AI) / Behavior Execution
 *  * Physics
 *  * Effects (particles, highlighting)
 *  * Lite UI (health bar)
 * Ideally the above functionality will be organized into components as 
 * development progresses.
 * 
 * Any other UI elements (e.g. menus) should be handled by Observers.
 * **/
public abstract class Unit : MonoBehaviour, IObservable
{
    // **         //
    // * FIELDS * //
    //         ** //

    public MeshRenderer highlightInner;
    public MeshRenderer highlightOuter;

    // unit management
    protected List<IObserver> observers;
    // physics
    protected Rigidbody body;
    protected Collider collision;
    // graphics
    protected MeshRenderer surface;
    // general
    protected Team team;
    protected string customName; // user-assigned names
    // balancing / gameplay
    protected ArmorType armorType;
    protected DamageType dmgType;
    protected float maxHealth;
    protected float health;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Notifies all observers.
    /// </summary>
    /// <param name="invocation">The name of the invocation.</param>
    /// <param name="data">Any additional data.</param>
    public void NotifyAll(Invocation invocation, params object[] data)
    {
        foreach (IObserver o in observers){
            o.OnNotify(this, invocation, data);
        }
    }

    /// <summary>
    /// Highlights the unit.
    /// </summary>
    public void Highlight()
    {
        highlightInner.enabled = true;
        highlightOuter.enabled = true;
    }

    /// <summary>
    /// Removes highlight from the unit.
    /// </summary>
    public void RemoveHighlight()
    {
        highlightInner.enabled = false;
        highlightOuter.enabled = false;
    }

    /// <summary>
    /// Sets up Observers and other state common between Units. Only needs to 
    /// be called once.
    /// </summary>
    public virtual void Build()
    {
        observers = new List<IObserver>
        {
            Toolbox.GameObserver,
            Toolbox.UIObserver
        };

    }

    /// <summary>
    /// Needs to be called every time a Unit is retrieved from its Object Pool.
    /// </summary>
    public virtual void Activate()
    {
        health = maxHealth;
        surface = GetComponent<MeshRenderer>();
        surface.material.color = Color.Lerp(Color.black, team.color, health / maxHealth);
    }

    /// <summary>
    /// Should be called whenever this unit is removed from play.
    /// </summary>
    public virtual void Deactivate()
    {
        RemoveHighlight();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Changes the health by a specified amount, and kills the unit if its 
    /// health is below zero.
    /// </summary>
    /// <param name="change">Damage to Take.</param>
    public virtual void UpdateHealth(float change, Unit source = null)
    {
        health += change;
        surface.material.color = Color.Lerp(Color.black, team.color, health / MaxHealth);
        if (health <= 0) { OnDeath(source); }
    }

    /// <summary>
    /// Returns the "identity" of the unit, a unique identifier for the purpose
    /// of disambiguation.
    /// </summary>
    public abstract string Identity();

    /// <summary>
    /// Returns this unit's cost, in gold.
    /// </summary>
    public abstract int Cost();

    /// <summary>
    /// All units must have code for what they do when another object collides 
    /// with them, but this behavior may vary from unit to unit, or be 
    /// otherwise type-specific.
    /// </summary>
    protected abstract void OnCollisionEnter(Collision collision);
    
    /// <summary>
    /// All units must have code to handle what happens when they die. At the 
    /// very least, the incident should be logged (TODO).
    /// </summary>
    /// <param name="killer">The unit's killer.</param>
    protected abstract void OnDeath(Unit killer);

    /// <summary>
    /// Gets the Team of the unit.
    /// </summary>
    public Team Team
    {
        get { return team; }
        set { team = value; }
    }

    /// <summary>
    /// Gets the name of the unit.
    /// </summary>
    public string Name
    {
        get
        {
            if (!(customName == null))
            {
                return customName;
            }
            return Identity();
        }
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
    /// Sets the custom name of this Unit.
    /// </summary>
    public string CustomName
    {
        set { customName = value; }
    }

    /// <summary>
    /// Gets the unit's Maximum Health.
    /// </summary>
    public float MaxHealth
    {
        get { return maxHealth; }
    }

    /// <summary>
    /// Gets / sets the unit's current Health.
    /// </summary>
    public float Health
    {
        get { return health; }
        set { health = value; }
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
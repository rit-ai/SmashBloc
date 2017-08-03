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
 * Any other UI elements (e.g. menus) should be handled by Observers.
 * **/
public abstract class Unit : MonoBehaviour, IObservable {

    // public fields
    public MeshRenderer m_HighlightInner;
    public MeshRenderer m_HighlightOuter;

    // protected fields related to unit management
    protected List<IObserver> observers;

    // protected fields intended to be changed for balancing or by gameplay
    protected string unitName;
    protected string customName; // user-assigned names
    protected float maxHealth;
    protected float health;

    // protected fields related to fundamentals of unit type
    protected ArmorType armorType;
    protected DamageType dmgType;

    // protected fields related to physics
    protected Rigidbody body;
    protected Collider collision;

    // protected fields related to graphics
    protected MeshRenderer m_Surface;

    // protected fields related to behavior
    protected Team team;

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
    /// Sets up Observers and other state common between Units.
    /// </summary>
    public virtual void Init()
    {
        observers = new List<IObserver>
        {
            Toolbox.GameObserver,
            Toolbox.UIObserver
        };

    }

    public virtual void Activate()
    {
        health = maxHealth;
        unitName = Identity();
        m_Surface = GetComponent<MeshRenderer>();
        m_Surface.material.color = Color.Lerp(Color.black, team.color, health / maxHealth);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Highlights the unit.
    /// </summary>
    public void Highlight()
    {
        m_HighlightInner.enabled = true;
        m_HighlightOuter.enabled = true;
    }

    /// <summary>
    /// Removes highlight from the unit.
    /// </summary>
    public void RemoveHighlight()
    {
        m_HighlightInner.enabled = false;
        m_HighlightOuter.enabled = false;
    }

    /// <summary>
    /// Attack the specified target.
    /// </summary>
    /// <param name="target">Target to attack.</param>
    public virtual void Attack(Unit target)
    {

    }

    /// <summary>
    /// Deal specified damage, and Kill() if applicable.
    /// </summary>
    /// <param name="change">Damage to Take.</param>
    public virtual void UpdateHealth(float change, Unit source = null)
    {

        health += change;
        m_Surface.material.color = Color.Lerp(Color.black, team.color, health / MaxHealth);
        if (health <= 0) { OnDeath(source); }
    }

    // Properties
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
    public string UnitName
    {
        get {
            if (!(customName == null))
            {
                return customName;
            }
            return unitName;
        }
    }

    /// <summary>
    /// Sets the default unit name.
    /// </summary>
    /// <param name="newName"></param>
    public void SetName(string newName)
    {
        unitName = newName;
    }

    /// <summary>
    /// Sets a permanent custom name for this unit.
    /// </summary>
    public void SetCustomName(string newName)
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
    /// Gets / sets the unit's current Health.
    /// </summary>
    public float Health
    {
        get { return health; }
        set { health = value; }
    }


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
    /// Returns the "identity" of the unit, a unique identifier for the purpose
    /// of disambiguation.
    /// </summary>
    public abstract string Identity();

    /// <summary>
    /// Returns this unit's cost, in gold.
    /// </summary>
    public abstract int Cost();

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
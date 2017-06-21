using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
 * Any other UI elements should be handled by Observers.
 * **/
public abstract class Unit : MonoBehaviour, Observable {

    // public fields
    public MeshRenderer m_HighlightInner;
    public MeshRenderer m_HighlightOuter;
    public int cost;

    // protected fields related to unit management
    protected List<Observer> observers;

    // protected fields intended to be changed for balancing or by gameplay
    protected string team;
    protected string unitName;
    protected string customName; // user-assigned names
    protected float maxHealth;
    protected float health;
    protected float dmg;
    protected float range;

    // protected fields related to fundamentals of unit type
    protected ArmorType armorType;
    protected DamageType dmgType;

    // protected fields related to physics
    protected Rigidbody body;
    protected Collider collision;

    // protected fields related to behavior
    protected Vector3 destination;

    /// <summary>
    /// Sets up Observers and other state common between Units.
    /// </summary>
    protected void Init()
    {
        observers = new List<Observer>();
        observers.Add(new MenuObserver());

        // Sets default destination to be the location the unit spawns
        SetDestination(transform.position);
    }

    /// <summary>
    /// Notifies all observers.
    /// </summary>
    /// <param name="invocation">The name of the invocation.</param>
    /// <param name="data">Any additional data.</param>
    public void NotifyAll<T>(string invocation, params T[] data)
    {
        foreach (Observer o in observers){
            o.OnNotify<T>(this, invocation);
        }
    }

    /// <summary>
    /// Logic handler for when the unit is individually selected, including
    /// notifying proper menu observers.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll<VoidObject>(MenuObserver.INVOKE_UNIT_DATA);
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
    public void setUnitName(string newName)
    {
        unitName = newName;
    }

    /// <summary>
    /// Sets a permanent custom name for this unit.
    /// </summary>
    public void setCustomName(string newName)
    {
        customName = newName;
    }

    /// <summary>
    /// Sets a new destination, which the unit will attempt to navigate toward.
    /// </summary>
    /// <param name="newDest"></param>
    public void SetDestination(Vector3 newDest)
    {
        destination = newDest;
    }

    /// <summary>
    /// Returns this unit's destination.
    /// </summary>
    public Vector3 Destination
    {
        get
        {
            return destination;
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



    /// <summary>
    /// Returns the "identity" of the unit, a unique identifier for the purpose
    /// of disambiguation.
    /// </summary>
    public abstract string Identity();

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
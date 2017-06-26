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
    public LayerMask ignoreAllButUnits;
    public bool ownedByPlayer;
    public int cost;

    // protected fields related to unit management
    protected List<Observer> observers;

    // protected fields intended to be changed for balancing or by gameplay
    protected string team;
    protected string unitName;
    protected string customName; // user-assigned names
    protected float maxHealth;
    protected float health;
    protected float damage;
    protected float sightRange;
    protected float attackRange;

    // protected fields related to fundamentals of unit type
    protected ArmorType armorType;
    protected DamageType dmgType;
    protected UnitAI ai;

    // protected fields related to physics
    protected Rigidbody body;
    protected Collider collision;

    // protected fields related to behavior
    protected Vector3 destination;

    // Private fields
    private EnvironmentInfo info;
    private MeshRenderer m_Surface;

    /// <summary>
    /// Sets up Observers and other state common between Units.
    /// </summary>
    public virtual void Start()
    {
        observers = new List<Observer>();
        observers.Add(new UIObserver());
        info = new EnvironmentInfo();
        m_Surface = GetComponent<MeshRenderer>();
        if (ownedByPlayer)
        {
            m_Surface.material.color = Color.blue;
            team = Utils.PlayerTeamName;
        }
        else
        {
            m_Surface.material.color = Color.red;
            team = "AI_TEAM"; //FIXME
        }

        // Set the AI component to update its state every second
        InvokeRepeating("passInfoToAI", 1f, 1f);

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
    /// Grabs all relevant information and builds it into an EnvironmentInfo 
    /// struct to pass into the unit's AI component.
    /// </summary>
    protected void passInfoToAI()
    {
        // Add all units within line of sight to the unitsInSightRange list.
        Unit current;
        List<Unit> enemiesInSight = new List<Unit>();
        List<Unit> alliesInSight = new List<Unit>();
        List<Unit> enemiesInAttackRange = new List<Unit>();
        List<Collider> collidersInSight;
        collidersInSight = new List<Collider>(Physics.OverlapSphere(transform.position, sightRange, ignoreAllButUnits));
        foreach (Collider c in collidersInSight)
        {
            current = c.gameObject.GetComponent<Unit>();
            // Only be aggressive to units on the other team.
            if (current.team != team)
            {
                // If they're close enough to attack, add them to the second list.
                if (c.transform.position.magnitude - transform.position.magnitude < attackRange)
                    enemiesInAttackRange.Add(current);
                enemiesInSight.Add(current);
            }
            else
            {
                alliesInSight.Add(current);
            }
        }

        // Build the info struct.
        info.team = team;
        info.healthPercentage = health / maxHealth;
        info.damage = damage;

        info.enemiesInSight = enemiesInSight;
        info.alliesInSight = alliesInSight;
        info.enemiesInAttackRange = enemiesInAttackRange;

        ai.UpdateState(info);
    }

    /// <summary>
    /// Logic handler for when the unit is individually selected, including
    /// notifying proper menu observers.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll<VoidObject>(UIObserver.INVOKE_UNIT_DATA);
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
        get { return damage; }
    }

    /// <summary>
    /// Gets the unit's Range.
    /// </summary>
    public float Range
    {
        get { return attackRange; }
    }

    /// <summary>
    /// Gets the Cost of the unit.
    /// </summary>
    public int Cost
    {
        get { return cost; }
    }

    /// <summary>
    /// Sets a new destination, which the unit will attempt to navigate toward.
    /// </summary>
    /// <param name="newDest"></param>
    public abstract void SetDestination(Vector3 newDest);

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
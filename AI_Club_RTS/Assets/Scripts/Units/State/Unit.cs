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
    public LayerMask ignoreAllButUnits;

    // protected fields related to unit management
    protected List<IObserver> observers;

    // protected fields intended to be changed for balancing or by gameplay
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
    protected Vector3 newPos;

    // protected fields related to behavior
    protected Team team;
    protected Vector3 destination;
    protected bool alive;

    // Private constants
    private const float PASS_INFO_RATE = 1f;

    // Private fields
    private MeshRenderer m_Surface;
    private UnitInfo info;

    /// <summary>
    /// Sets up Observers and other state common between Units.
    /// </summary>
    protected virtual void Start()
    {

        observers = new List<IObserver>
        {
            gameObject.AddComponent<GameObserver>()
        };

        info = new UnitInfo();
        alive = true;

        // Pass info to the AI component every second
        StartCoroutine(PassInfo());
    }

    /// <summary>
    /// Handles any processing that must occur only AFTER the Unit is 
    /// instantiated. For example, a Unit can only be told what team it's on
    /// after it's been created.
    /// </summary>
    public void Init(Team team)
    {
        m_Surface = GetComponent<MeshRenderer>();

        this.team = team;
        m_Surface.material.color = team.color;
    }

    /// <summary>
    /// Notifies all observers.
    /// </summary>
    /// <param name="invocation">The name of the invocation.</param>
    /// <param name="data">Any additional data.</param>
    public void NotifyAll(Invocation invocation, params object[] data)
    {
        foreach (IObserver o in observers){
            o.OnNotify(this, invocation);
        }
    }

    /// <summary>
    /// Grabs all relevant information and builds it into an EnvironmentInfo 
    /// struct to pass into the unit's AI component.
    /// </summary>
    protected IEnumerator PassInfo()
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

        ai.UpdateInfo(info);
        yield return new WaitForSeconds(PASS_INFO_RATE);
    }

    /// <summary>
    /// Logic handler for when the unit is individually selected, including
    /// notifying proper menu observers.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll(Invocation.UNIT_MENU);
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
    /// <param name="damage">Damage to Take.</param>
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        m_Surface.material.color = Color.Lerp(Team.color, Color.black, health / MaxHealth);
        if (health <= 0f) { health = 0f; Kill(); }
    }

    /// <summary>
    /// Kill this instance.
    /// </summary>
    public void Kill()
    {
        alive = false;
        StartCoroutine(DeathAnimation());
    }

    // Properties
    /// <summary>
    /// Gets the Team of the unit.
    /// </summary>
    public Team Team
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
    /// Returns this unit's destination.
    /// </summary>
    public Vector3 Destination
    {
        get { return destination; }
        set { destination = value; }
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
    /// All units must have code for what they do when another object collides 
    /// with them, but this behavior may vary from unit to unit, or be 
    /// otherwise type-specific.
    /// </summary>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null && !(unit.Team.Equals(team)))
        {
            TakeDamage(Random.Range(10f, 20f));
        }
        City city = collision.gameObject.GetComponent<City>();
        if (city != null && !(city.Team.Equals(team)))
        {
            TakeDamage(Random.Range(10f, 20f));
        }
    }

    /// <summary>
    /// "Animates" the death of the unit, which can be handled as the 
    /// implementer sees fit. The default behavior is to become very heavy and
    /// then fade out.
    /// </summary>
    protected virtual IEnumerator DeathAnimation()
    {
        Color fadeOut = m_Surface.material.color;

        GetComponent<Rigidbody>().mass *= 100;
        for (float x = 1; x > 0; x -= 0.01f)
        {
            fadeOut.a = x;
            m_Surface.material.color = fadeOut;
            yield return 0f;
        }

        Destroy(gameObject);

        yield return null;
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
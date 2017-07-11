using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This abstract class represents Units that can move.
 * **/
public abstract class MobileUnit : Unit
{
    public LayerMask ignoreAllButUnits;

    protected UnitAI ai;
    protected Vector3 newPos;
    protected Vector3 destination;
    protected bool alive;
    protected float damage;
    protected float sightRange;
    protected float attackRange;

    // Private constants
    private const float PASS_INFO_RATE = 1f;

    // Private fields
    private MobileUnitInfo info;

    protected override void Start()
    {
        base.Start();
        alive = true;
        destination = team.cities[0].transform.position;
        info = new MobileUnitInfo();

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
    /// Grabs all relevant information and builds it into an EnvironmentInfo 
    /// struct to pass into the unit's AI component.
    /// </summary>
    protected IEnumerator PassInfo()
    {
        // Add all units within line of sight to the unitsInSightRange list.
        MobileUnit current;
        List<MobileUnit> enemiesInSight = new List<MobileUnit>();
        List<MobileUnit> alliesInSight = new List<MobileUnit>();
        List<MobileUnit> enemiesInAttackRange = new List<MobileUnit>();
        List<Collider> collidersInSight;
        collidersInSight = new List<Collider>(Physics.OverlapSphere(transform.position, sightRange, ignoreAllButUnits));
        foreach (Collider c in collidersInSight)
        {
            current = c.gameObject.GetComponent<MobileUnit>();
            // Only be aggressive to units on the other team.
            if (current.Team != team)
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
    /// Kill this instance.
    /// </summary>
    protected override void OnDeath(Unit killer)
    {
        alive = false;
        StartCoroutine(DeathAnimation());
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

    protected override void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null && !(unit.Team.Equals(team)))
        {
            base.TakeDamage(UnityEngine.Random.Range(10f, 20f), unit);
        }
    }

    public abstract override int Cost();

    public abstract override string Identity();

    /// <summary>
    /// Returns this unit's destination.
    /// </summary>
    public Vector3 Destination
    {
        get { return destination; }
        set { destination = value; }
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
}

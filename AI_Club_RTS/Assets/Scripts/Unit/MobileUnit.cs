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

    protected MobileAI ai;
    protected Vector3 newPos;
    protected Vector3 destination;
    protected Vector3 storedDestination = default(Vector3);
    protected float damage;
    protected float sightRange;
    protected float attackRange;

    // Private constants
    private const float PASS_INFO_RATE = 1f;

    // Private fields
    private MobileUnitInfo info;

    // Set initial state for when a MobileUnit is created
    public override void Activate()
    {
        destination = transform.position;
        // Pass info to the AI component every second
        info = new MobileUnitInfo();
        StartCoroutine(PassInfo());

        base.Activate();
    }


    /// <summary>
    /// Causes the unit's health to become zero.
    /// </summary>
    public void ForceKill()
    {
        health = 0;
        OnDeath(null);
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

        // Build the info object.
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



        yield return null;
    }

    /// <summary>
    /// Units take damage when they collide with a unit of the enemy team.
    /// </summary>
    protected override void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null && !(unit.Team.Equals(team)))
        {
            base.TakeDamage(UnityEngine.Random.Range(10f, 20f), unit);
        }
    }

    /// <summary>
    /// All Mobile Units must be able to shoot at other Units.
    /// </summary>
    public abstract IEnumerator Shoot(Unit target, float maxAimTime);

    public abstract override int Cost();

    public abstract override string Identity();

    /// <summary>
    /// The Unit's brain.
    /// </summary>
    public MobileAI AI
    {
        get { return ai; }
        set { ai = value; }
    }

    /// <summary>
    /// This unit's current destination.
    /// </summary>
    public Vector3 Destination
    {
        get { return destination; }
        set {
            // Don't change the destination if we're currently waiting to fire
            if (storedDestination != default(Vector3)) {
                value.y = 0;
                storedDestination = value;
            }
            else
            {
                value.y = 0;
                destination = value;
            }
        }
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

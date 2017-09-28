using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This abstract class represents Units that can move (as opposed to buildings, 
 * like Cities). This type of unit is the core of a Player's capabilities and
 * influence in the game. Whether or not they're effective depends largely on 
 * the type of brain they've been given.
 * **/
public abstract class MobileUnit : Unit
{
    // **         //
    // * FIELDS * //
    //         ** //

    protected MobileAI brain;
    protected Vector3 movingTo; // the exact position we're moving to
    protected Vector3 pointOfInterest; // the location of where we want to go
    protected float damage;
    protected float sightRange;
    protected float attackRange;

    private const float PASS_INFO_RATE = 1f;

    private MobileInfo info;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Causes the unit's health to become zero.
    /// </summary>
    public void ForceKill()
    {
        health = 0;
        OnDeath(null);
    }

    /// <summary>
    /// Activates the Mobile and initializes its brain.
    /// </summary>
    public override void Activate()
    {
        // Units wait near their spawn position until given orders
        pointOfInterest = transform.position;

        // Pass info to the AI component every second
        info = new MobileInfo();
        StartCoroutine(PassInfo());

        brain.Activate();
        base.Activate();
    }

    /// <summary>
    /// Deactivates the Mobile and removes it from its team.
    /// </summary>
    public override void Deactivate()
    {
        team.mobiles.Remove(this);
        base.Deactivate();
    }

    /// <summary>
    /// Grabs all relevant information and builds it into an EnvironmentInfo 
    /// struct to pass into the unit's AI component.
    /// </summary>
    protected IEnumerator PassInfo()
    {
        // Add all units within line of sight to the unitsInSightRange list.
        while (true)
        {
            Unit current;
            List<Unit> enemiesInSight = new List<Unit>();
            List<Unit> alliesInSight = new List<Unit>();
            List<Unit> enemiesInAttackRange = new List<Unit>();
            List<Collider> collidersInSight = new List<Collider>(Physics.OverlapSphere(transform.position, sightRange, (Toolbox.UnitLayer | Toolbox.MobileLayer)));
            foreach (Collider c in collidersInSight)
            {
                current = c.gameObject.GetComponent<Unit>();
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

            info.movingTo = movingTo;
            info.pointOfInterest = pointOfInterest;

            brain.Info = info;
            yield return new WaitForSeconds(PASS_INFO_RATE);
        }

    }


    /// <summary>
    /// Units take damage when they collide with a unit of the enemy team.
    /// </summary>
    protected override void OnCollisionEnter(Collision collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if (unit != null && !(unit.Team.Equals(team)))
        {
            base.UpdateHealth(-UnityEngine.Random.Range(10f, 20f), unit);
        }
    }

    /// <summary>
    /// Kill this instance.
    /// </summary>
    protected override void OnDeath(Unit killer)
    {
        if (gameObject.activeSelf) { StartCoroutine(DeathAnimation()); }
    }

    /// <summary>
    /// "Animates" the death of the unit, which can be handled as the 
    /// implementer sees fit. The default behavior is to become very heavy and
    /// then fade out.
    /// </summary>
    protected virtual IEnumerator DeathAnimation()
    {
        Color fadeOut = surface.material.color;

        GetComponent<Rigidbody>().mass *= 100;
        for (float x = 1; x > 0; x -= 0.01f)
        {
            fadeOut.a = x;
            surface.material.color = fadeOut;
            yield return 0f;
        }
    }

    /// <summary>
    /// All Mobile Units must be able to shoot at other Units.
    /// </summary>
    public abstract IEnumerator Shoot(Unit target, float maxAimTime);

    public abstract override int Cost();

    public abstract override string Identity();

    /// <summary>
    /// Logic handler for when the unit is individually selected, including
    /// notifying proper menu observers.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll(Invocation.ONE_SELECTED);
        NotifyAll(Invocation.UNIT_MENU);
    }

    /// <summary>
    /// The Unit's brain.
    /// </summary>
    public MobileAI AI
    {
        get { return brain; }
        set { brain = value; }
    }

    /// <summary>
    /// This unit's current destination. Its Y component is removed.
    /// </summary>
    public Vector3 Destination
    {
        get { return movingTo; }
        set { movingTo = value; }
    }

    public Vector3 PointOfInterest
    {
        set { pointOfInterest = value; }
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

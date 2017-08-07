using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Ben Fairlamb
 * @author Paul Galatic
 * 
 * Class designed to handle City-specific functionality and state. Like with 
 * other game objects, menus and UI elements should be handled by observers.
 * **/
public class City : Unit
{

    // Public constants
    public const string IDENTITY = "CITY";
    public const float MAX_HEALTH = 500f;
    public const int MAX_INCOME_LEVEL = 8;

    // Public fields
    public Transform spawnPoint;
    public SpawnRingRig spawnRingRig;

    // Private constants
    private const string DEFAULT_NAME = "Dylanto";
    // The health a city has just after it's captured
    private const float CAPTURED_HEALTH = 50f;
    // The rate at which a city's health regenerates (per second)
    private const float REGENERATION_RATE = CAPTURED_HEALTH / 5f;
    // Regeneration is delayed after taking damage
    private const float REGENERATION_DELAY = 2f;
    // Enemy units will suffer recoil upon contact with a City
    private const float BLOWBACK_FORCE = 30f;
    private const float BLOWBACK_RADIUS = 10f;
    private const int COST = 500;
    private const int MIN_INCOME_LEVEL = 1;
    private const int DEFAULT_INCOME_LEVEL = 8;

    // Private fields
    private int incomeLevel;
    private bool delayRegen = true;

    public override void Activate()
    {
        maxHealth = MAX_HEALTH;
        // Default values
        incomeLevel = DEFAULT_INCOME_LEVEL;

        StartCoroutine(Regenerate());

        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
        team.cities.Remove(this);
        Toolbox.CityPool.Return(this);
    }

    /// <summary>
    /// Causes the city to lose health. 
    /// </summary>
    /// <param name="damage">The amount of damage to take.</param>
    /// <param name="source">The source of the damage.</param>
    public override void UpdateHealth(float damage, Unit source)
    {
        delayRegen = true;
        base.UpdateHealth(damage, source);
    }

    /// <summary>
    /// Returns the location at which to spawn units.
    /// </summary>
    public Transform SpawnPoint
    {
        get { return spawnPoint; }
    }

    /// <summary>
    /// Gets the Income Level of the city.
    /// </summary>
    public int IncomeLevel
    {
        get { return incomeLevel; }
    }

    /// <summary>
    /// Returns the class name of the unit in the form of a string.
    /// </summary>
    public override string Identity()
    {
        return IDENTITY;
    }

    /// <summary>
    /// Returns how much a city would cost to place.
    /// </summary>
    public override int Cost()
    {
        return COST;
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
            Rigidbody body = collision.gameObject.GetComponent<Rigidbody>();
            // Add audiovisuals here
            body.AddExplosionForce(BLOWBACK_FORCE, transform.position - body.transform.position, BLOWBACK_RADIUS);
            UpdateHealth(-UnityEngine.Random.Range(10f, 20f), unit);
        }
    }

    /// <summary>
    /// If the city's health goes to or below 
    /// zero, its team changes to the team that caused the damage, and its 
    /// health gets set to a CAPTURED_HEALTH (to prevent rapid capturing / 
    /// recapturing in the event of a major skirmish).
    /// </summary>
    /// <param name="capturer">The capturer of the city.</param>
    protected override void OnDeath(Unit capturer)
    {
        ChangeColor(capturer.Team.color);
        UpdateHealth(CAPTURED_HEALTH - health, capturer);
        NotifyAll(Invocation.CITY_CAPTURED, capturer.Team);
    }

    /// <summary>
    /// Changes the city's color.
    /// </summary>
    /// <param name="color"></param>
    private void ChangeColor(Color color)
    {
        surface.material.color = color;
        spawnRingRig.UpdateColor(color);
    }

    /// <summary>
    /// Cities will regenerate their health over time.
    /// </summary>
    private IEnumerator Regenerate()
    {
        WaitForSeconds wait = new WaitForSeconds(REGENERATION_DELAY);
        while (true)
        {
            if (delayRegen)
            {
                delayRegen = false;
                yield return wait;
            }
            base.UpdateHealth(REGENERATION_RATE * Time.deltaTime);
            yield return 0f;
        }
    }

    /// <summary>
    /// Pull up the menu when the unit is clicked.
    /// </summary>
    private void OnMouseDown()
    {
        Highlight();
        NotifyAll(Invocation.CITY_MENU);
    }
}

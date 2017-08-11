using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Ben Fairlamb
 * @author Paul Galatic
 * 
 * The Twirl is the most basic unit of a Player's arsenal.
 * **/
public class Twirl : MobileUnit
{
    // **         //
    // * FIELDS * //
    //         ** //

    public const string IDENTITY = "Twirl";
    public const int COST = 50;

    public Rigidbody hoverBall;
    public Rigidbody bottomWeight;

    private const ArmorType ARMOR_TYPE = ArmorType.M_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;
    private const float ASCENSION_HEIGHT = 800f;
    private const float ASCENSION_DURATION = 20f;
    private const float DEST_DEVIATION_RADIUS = 50f;
    private const float MAXHEALTH = 100f;
    private const float DAMAGE = 10f;
    private const float SIGHT_RANGE = 500f;
    private const float RANGE = 50f;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Used for one-time initialization.
    /// </summary>
    public override void Build () {
        // Handle default values
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        maxHealth = MAXHEALTH;
        damage = DAMAGE;
        sightRange = SIGHT_RANGE;
        attackRange = RANGE;

        base.Build();
    }

    /// <summary>
    /// Prompts an Twirl unit to aim at another Unit, shooting either when
    /// locked on or after a specified amount of time elapses.
    /// </summary>
    /// <param name="target">The Unit to shoot at.</param>
    /// <param name="maxAimTime"></param>
    /// <returns></returns>
    public override IEnumerator Shoot(Unit target, float maxAimTime)
    {
        throw new NotImplementedException();

        /*
        float timeLeft = maxAimTime;
        // Twirl cannot navigate to a destination and aim at the same time, 
        // so the destination is temporarily stored.
        storedDestination = destination;

        while (timeLeft > 0f)
        {
            // aim
            // if (foundTarget) {break;}
            timeLeft -= Time.deltaTime;
            yield return null; // waits for next frame
        }

        // shoot

        destination = storedDestination;
        storedDestination = default(Vector3);
        */
    }

    public override void Deactivate()
    {
        base.Deactivate();
        Toolbox.TwirlPool.Return(this);
    }

    /// <summary>
    /// Returns identity of the unit, for disambiguation purposes.
    /// </summary>
    public override string Identity()
    {
        return IDENTITY;
    }

    /// <summary>
    /// The cost of an Twirl unit, in gold.
    /// </summary>
    public override int Cost()
    {
        return COST;
    }

    /// <summary>
    /// Effects are TODO. This will likely take the form of an animation.
    /// </summary>
    protected override IEnumerator DeathAnimation()
    {
        Deactivate();

        yield return null;
    }


}

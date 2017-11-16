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
    public MeshRenderer laserfan;
    public Laser laser;

    private const ArmorType ARMOR_TYPE = ArmorType.M_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;
    private const float ASCENSION_HEIGHT = 800f;
    private const float ASCENSION_DURATION = 20f;
    private const float DEST_DEVIATION_RADIUS = 50f;
    private const float MAXHEALTH = 100f;
    private const float DAMAGE = 10f;
    private const float SIGHT_RANGE = 150f;
    private const float RANGE = 150f;

    // **          //
    // * METHODS * //
    //          ** //

    /// <summary>
    /// Used for one-time initialization.
    /// </summary>
    public override void Build () {
        base.Build();
        
        // Handle default values
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        maxHealth = MAXHEALTH;
        damage = DAMAGE;
        sightRange = SIGHT_RANGE;
        attackRange = RANGE;

        surfaces.Add(laserfan);
        laser = GetComponentInChildren<Laser>();
    }

    /// <summary>
    /// Skeleton function to allow the aiming coroutine to start.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="maxAimTime"></param>
    public override void Shoot(Unit target, float maxAimTime)
    {
        StartCoroutine(AimShoot(target, maxAimTime));
    }

    /// <summary>
    /// Prompts an Twirl unit to aim at another Unit, shooting either when
    /// locked on or after a specified amount of time elapses.
    /// </summary>
    /// <param name="target">The Unit to shoot at.</param>
    /// <param name="aimTime"></param>
    /// <returns></returns>
    public IEnumerator AimShoot(Unit target, float aimTime = 1f)
    {
        while (aimTime > 0f)
        {
            // aim
            Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
            aimTime -= Time.deltaTime;
            yield return null; // waits for next frame
        }

        // shoot
        laser.Shoot(attackRange, damage);
        
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

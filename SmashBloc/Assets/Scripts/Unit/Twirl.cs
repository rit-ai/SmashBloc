using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twirl : MobileUnit {
    // Author: Ben Fairlamb
    // Author: Paul Galatic
    // Purpose: Twirly!

    // Public constants
    public const string IDENTITY = "Twirl";
    public const int COST = 50;

    // Public fields
    public Rigidbody m_Hoverball;
    public Rigidbody m_BottomWeight;

    // Private constants
    private const ArmorType ARMOR_TYPE = ArmorType.M_ARMOR;
    private const DamageType DMG_TYPE = DamageType.BULLET;
    private const float ASCENSION_HEIGHT = 500f;
    private const float ASCENSION_DURATION = 2f;
    // Default values
    private const float DEST_DEVIATION_RADIUS = 50f;
    private const float MAXHEALTH = 100f;
    private const float DAMAGE = 10f;
    private const float SIGHT_RANGE = 500f;
    private const float RANGE = 50f;

    // Methods

    // Use this for initialization
    public new void Init () {
        // Handle default values
        armorType = ARMOR_TYPE;
        dmgType = DMG_TYPE;
        maxHealth = MAXHEALTH;
        damage = DAMAGE;
        sightRange = SIGHT_RANGE;
        attackRange = RANGE;

        base.Init();
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
    /// In this animation, the Twirl unit sails into the air before being
    /// destroyed. Particle effects are TODO.
    /// </summary>
    protected override IEnumerator DeathAnimation()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        Color fadeOut = m_Surface.material.color;
        float dest = transform.position.y + ASCENSION_HEIGHT;
        float ascension_rate = ASCENSION_HEIGHT / ASCENSION_DURATION;
        while (transform.position.y < dest)
        {
            newPos = transform.position;
            newPos.y += ascension_rate * Time.deltaTime;
            transform.position = newPos;

            fadeOut.a -= 3f;
            m_Surface.material.color = fadeOut;
            yield return 0f;
        }

        Toolbox.TwirlPool.Return(this);

        yield return null;
    }

}

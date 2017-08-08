using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to represent all the state that should be passed to a 
 * MobileAI in order for it to evaluate its current situation and make a 
 * decision. This struct should NOT contain anything particular to a specific
 * type of unit, as this data will be passed uniformly to all Mobiles.
 * **/
public struct MobileInfo {

    // **         //
    // * FIELDS * //
    //         ** //

    // Any enemies that are in the Unit's sight range.
    public List<Unit> enemiesInSight;
    // Any allies that are in the Unit's sight range.
    public List<Unit> alliesInSight;
    // Any enemies that are in the Unit's attack range.
    public List<Unit> enemiesInAttackRange;

    // The team of the unit.
    public Team team;
    // The relative amount of health the unit currently possesses.
    public float healthPercentage;
    // The amount of damage the unit's weapon does.
    public float damage;
    // The current destination of the unit.
    public Vector3 movingTo;

    // The unit's current point of interest.
    public Vector3 pointOfInterest;

}

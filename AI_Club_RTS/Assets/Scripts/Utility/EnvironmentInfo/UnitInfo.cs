using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to represent all the state that should be passed to a 
 * UnitAI in order for it to evaluate its current situation and make a 
 * decision. This struct should NOT contain anything particular to a specific
 * type of unit, as different AIs will be implemented for different Unit types.
 * 
 * This is a class, as opposed to a struct, because it must be nullable in 
 * order to play nice with the AI heirarchy.
 * **/
public class UnitInfo {
    // The team of the unit.
    public Team team;
    // The relative amount of health the unit currently possesses.
    public float healthPercentage;
    // The amount of damage the unit's weapon does.
    public float damage;
    // The current destination of the unit.
    public Vector3 destination;

    // Any enemies that are in the Unit's sight range.
    public List<Unit> enemiesInSight;
    // Any allies that are in the Unit's sight range.
    public List<Unit> alliesInSight;
    // Any enemies that are in the Unit's attack range.
    public List<Unit> enemiesInAttackRange;

    public UnitInfo() { }

    public UnitInfo(Team team, float healthPercentage, float damage, Vector3 destination, List<Unit> enemiesInSight, List<Unit> alliesInSight, List<Unit> enemiesInAttackRange)
    {
        this.team = team;
        this.healthPercentage = healthPercentage;
        this.damage = damage;
        this.destination = destination;

        this.enemiesInSight = enemiesInSight;
        this.alliesInSight = alliesInSight;
        this.enemiesInAttackRange = enemiesInAttackRange;
    }
}

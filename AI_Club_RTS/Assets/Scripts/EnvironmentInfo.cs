using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Struct designed to represent all the state that should be passed to a 
 * Unit_AI in order for it to evaluate its current situation and make a 
 * decision. This struct should NOT contain anything particular to a specific
 * type of unit, as different AIs will be implemented for different Unit types.
 * **/
public struct EnvironmentInfo {
    // The team of the unit.
    public Team team;
    // The relative amount of health the unit currently possesses.
    public float healthPercentage;
    // The amount of damage the unit's weapon does.
    public float damage;


    public List<Unit> enemiesInSight;
    public List<Unit> alliesInSight;
    public List<Unit> enemiesInAttackRange;

    public EnvironmentInfo(Team team, float healthPercentage, float damage, List<Unit> enemiesInSight, List<Unit> alliesInSight, List<Unit> enemiesInAttackRange)
    {
        this.team = team;
        this.healthPercentage = healthPercentage;
        this.damage = damage;

        this.enemiesInSight = enemiesInSight;
        this.alliesInSight = alliesInSight;
        this.enemiesInAttackRange = enemiesInAttackRange;
    }
}

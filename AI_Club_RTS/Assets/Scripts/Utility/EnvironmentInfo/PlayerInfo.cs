using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to represent all the state that should be passed to a 
 * PlayerAI in order for it to evaluate its current situation and make a 
 * decision. This struct should NOT contain anything particular to a specific
 * type of unit, as different AIs will be implemented for different Unit types.
 * 
 * This is a class, as opposed to a struct, because it must be nullable in 
 * order to play nice with the AI heirarchy.
 * **/
public class PlayerInfo {

    public Team team;

    public int goldAmount;

    public List<City> cities;
    public List<Unit> units;

    public PlayerInfo() { }

    public PlayerInfo(Team team, int goldAmount, List<City> cities, List<Unit> units)
    {
        this.team = team;
        this.goldAmount = goldAmount;
        this.cities = cities;
        this.units = units;
    }

}

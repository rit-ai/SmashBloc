using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * A struct that contains all relevant information denoting to which team a
 * unit or city belongs.
 * **/
public class Team {

    public readonly string title;
    public readonly Color color;

    public List<MobileUnit> mobiles;
    public List<City> cities;

    /// <summary>
    /// Constructs a new Team.
    /// </summary>
    /// <param name="owner">The owner of the team.</param>
    /// <param name="title">The name of the team.</param>
    /// <param name="color">The team's color.</param>
    public Team (string title = "NULL", Color color = default(Color))
    {
        this.title = title;
        this.color = color;
        mobiles = new List<MobileUnit>();
        cities = new List<City>();
    }

    public Team(string title, Color color, List<MobileUnit> mobiles, List<City> cities)
    {
        this.title = title;
        this.color = color;
        this.mobiles = mobiles;
        this.cities = cities;
    }

    /// <summary>
    /// Kills all units in a team.
    /// </summary>
    public void DestroyTeam()
    {
        foreach (MobileUnit m in mobiles)
        {
            m.ForceKill();
        };
    }

    public static bool operator ==(Team t1, Team t2)
    {
        return t1.Equals(t2);
    }

    public static bool operator !=(Team t1, Team t2)
    {
        return !(t1.Equals(t2));
    }

    public override bool Equals(object other) {
        if (!(other is Team)) { return false; }
        Team another = (Team)other;
        return (title == another.title) && (color == another.color);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

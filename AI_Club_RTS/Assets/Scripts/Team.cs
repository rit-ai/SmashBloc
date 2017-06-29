using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * A struct that contains all relevant information denoting to which team a
 * unit or city belongs.
 * **/
public struct Team {

    public readonly Player owner;
    public readonly string name;
    public readonly Color color;

    /// <summary>
    /// Constructs a new Team.
    /// </summary>
    /// <param name="owner">The owner of the team.</param>
    /// <param name="name">The name of the team.</param>
    /// <param name="color">The team's color.</param>
    public Team (Player owner, string name = "NULL", Color color = default(Color))
    {
        this.owner = owner;
        this.name = name;
        this.color = color;
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
        return ((owner == another.owner) && (name == another.name) && (color == another.color));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

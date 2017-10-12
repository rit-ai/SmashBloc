using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * @author Paul Galatic
 * 
 * Base class to represent the lower limit of what an AI is expected to 
 * perform. To be used as a "starter file" for those that wish to develop new
 * AI for units, rather than having to change an existing one (though the 
 * existing files may freely be used as examples).
 * 
 * A Unit will regularly receive information from its body. When this happens,
 * Decide() is called. At that point, it is the responsibility of the AI to 
 * construct Commands and use AddCommand() to relay said commands to the body.
 * **/
public sealed class MobileAI_Basic : MobileAI
{

    protected override IThought Think()
    {

        int shoot = 0;
        int flee = 0;
        int move = 0;

        shoot += info.enemiesInAttackRange.Count;
        flee += info.enemiesInSight.Count;
        shoot += info.alliesInSight.Count;
        move += 10;

        Dictionary<EThought, float> weightedVals = new Dictionary<EThought, float>
        {
            { EThought.SHOOT, shoot },
            { EThought.FLEE, flee },
            { EThought.MOVE, move }
        };

        EThought decision = StochasticChoice(weightedVals);

        switch (decision)
        {
            case EThought.SHOOT:
                return new Shoot(info.enemiesInAttackRange[0]);
            case EThought.FLEE:
                return new Flee(info.enemiesInSight[0]);
            case EThought.MOVE:
                return new Move(info.pointOfInterest);
            default:
                throw new Exception("Recieved a bad EThought in MobileAI_Basic.");
        }
        
    }
}

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

    protected override void Start()
    {
        base.Start();
    }

    protected override void Decide()
    {
        
    }
}

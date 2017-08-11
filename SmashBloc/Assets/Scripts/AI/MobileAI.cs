using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class created to represent state common across all Unit AIs and providing 
 * details on how to implement a custom one. The basic dataflow of a Mobile AI 
 * is as follows:
 * 
 * 1. Receive Info from Body
 * 2. Process Info and execute ICommand
 * 
 * Step one is called by the body in UpdateInfo(). Step two is Decide(). 
 * Children classes only have to worry about implementing Decide().
 * **/
public abstract class MobileAI : AbstractAI
{
    // Unit AIs control Units. However, the AIs aren't allowed to reference
    // their Units themselves—they can only do so through Commands.
    private MobileUnit body;
    // This is the most updated information the AI has from its body.
    protected MobileInfo info;

    public MobileUnit Body { set { body = value; } }
    // It's the Body's responsibility to keep this value updated, but it won't
    // be sampled more frequently than INFO_SAMPLING_RATE.
    public MobileInfo Info { set { info = value; } }

    /// <summary>
    /// Periodically makes a decision based on current known info.
    /// </summary>
    protected sealed override IEnumerator Thinking(float SAMPLING_RATE)
    {
        IThought thought;

        while (true)
        {
            // Use sensory info to determine a course of action
            thought = Think();
            // Beware of impure thoughts
            if (thought != null && thought is MobileThought)
            {
                // If the thought is pure, act on it
                ((MobileThought)thought).Body = body;
                thought.Act();
                priorThought = thought;
                thought = null;
            }
            else
            {
                Debug.Log("WARN: MobileAI has bad thoughts.");
            }

            yield return new WaitForSeconds(SAMPLING_RATE);
        }

    }

    /// <summary>
    /// The responsibility of this function is to evaluate the current info and 
    /// return a Thought intended to advance the Player's goals.
    /// </summary>
    protected abstract override IThought Think();
}

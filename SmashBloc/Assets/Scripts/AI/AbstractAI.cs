using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Abstract class that represents data common between the AI of different 
 * types.
 * **/
public abstract class AbstractAI : MonoBehaviour {

    private const float INFO_SAMPLING_RATE = 1f;

    // This is the most recent command that the Body executed. Useful if you
    // don't want to execute the same type of command again and again.
    protected IThought priorThought;

    /// <summary>
    /// Attempts to generate and act upon a Thought.
    /// </summary>
    protected abstract IEnumerator Thinking(float SAMPLING_RATE);

    /// <summary>
    /// Decide how to handle new information. Will be called after every update
    /// to info.
    /// </summary>
    protected abstract IThought Think();

    /// <summary>
    /// Given a dictionary of types, chooses one. Helpful for choosing from a 
    /// collection of IThoughts.
    /// 
    /// This algorithm is O(n) time where n is the size of weightedVals, so 
    /// don't overuse it.
    /// </summary>
    protected EThought StochasticChoice(Dictionary<EThought, float> weightedVals) 
    {
        float max = weightedVals.Sum(v => v.Value);
        float f = UnityEngine.Random.Range(0, max);
        float total = 0f;
        foreach (EThought t in weightedVals.Keys)
        {
            total += weightedVals[t];
            if (f < total) { return t; }
        }
        throw new Exception("Error in AbstractAI.StochasticChoice(): No value could be chosen");
    }

    // Start thinking.
    public void Activate()
    {
        StartCoroutine(Thinking(INFO_SAMPLING_RATE));
    }

    // Stop thinking.
    public void Deactivate()
    {
        StopAllCoroutines();
    }

}

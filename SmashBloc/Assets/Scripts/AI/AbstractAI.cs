using System;
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

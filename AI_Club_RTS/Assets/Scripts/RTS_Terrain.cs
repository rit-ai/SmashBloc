using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * Class designed to handle behavior related to the terrain of the world.
 * 
 * Naturally, any behavior outside the domain of terrain will be forwarded by
 * Observers and processed externally.
 * **/
public class RTS_Terrain : MonoBehaviour, IObservable
{

    // Public fields
    public LayerMask ignoreAllButTerrain;

    // Private fields
    private List<IObserver> m_Observers;

    public void Start()
    {
        m_Observers = new List<IObserver>
        {
            new UIObserver(),
            new GameObserver()
        };
    }

    public void NotifyAll(Invocation invocation, params object[] data)
    {
        foreach (IObserver o in m_Observers)
        {
            o.OnNotify(this, invocation, data);
        }
    }

    /// <summary>
    /// When the terrain is left clicked, close all menus.
    /// </summary>
    public void OnMouseDown()
    {
        if (Utils.MouseIsOverUI()) { return; }
        NotifyAll(Invocation.CLOSE_ALL);
    }

    /// <summary>
    /// When the terrain is right clicked, set a new destination for all 
    /// selected units.
    /// </summary>
    public void OnRightMouseDown()
    {
        NotifyAll(Invocation.TARGET_RING);
        NotifyAll(Invocation.DESTINATION_SET);
    }
}

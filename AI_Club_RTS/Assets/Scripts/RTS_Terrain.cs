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
public class RTS_Terrain : MonoBehaviour, Observable
{

    private List<Observer> m_Observers;

    public void Start()
    {
        m_Observers = new List<Observer>();
        m_Observers.Add(new MenuObserver());
    }

    public void NotifyAll<T>(string invocation, params T[] data)
    {
        foreach (Observer o in m_Observers)
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
        NotifyAll<VoidObject>(MenuObserver.CLOSE_ALL);

    }
}

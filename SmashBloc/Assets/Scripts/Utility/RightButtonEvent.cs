using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
[ExecuteInEditMode]
[AddComponentMenu("Event/RightButtonEvent")]
/*
 * @author jenci1990 · Jan 18, 2015
 * @author Paul Galatic
 * 
 * Script designed to hanlde right click events on game objects. To be added 
 * via the editor as necessary.
 * 
 * Source: http://answers.unity3d.com/questions/879156/how-would-i-detect-a-right-click-with-an-event-tri.html
 * **/
public class RightButtonEvent : MonoBehaviour
{
    // **         //
    // * FIELDS * //
    //         ** //

    [System.Serializable] public class RightButton : UnityEvent { }
    public RightButton onRightDown;
    public RightButton onRightUp;

    // **          //
    // * METHODS * //
    //          ** //

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            onRightDown.Invoke();
        }
        if (Input.GetMouseButtonUp(1))
        {
            onRightUp.Invoke();
        }
    }
}
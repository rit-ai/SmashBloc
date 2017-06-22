using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This class handles logic involving the movement of the camera, and player 
 * interaction with the world space (NOT menus).
 * 
 * If a menu is opened, CameraController defers to its observer, UIManager.
 * **/
public class CameraController : MonoBehaviour, Observable {

    // Public constants
    public static KeyCode DESELECT_KEY;

    // Public fields
    public Camera m_Camera;
    public LayerMask Terrain_mask;

    // Private constants
    private static Color BOX_INTERIOR_COLOR = new Color(0.74f, 0.71f, 0.27f, 0.5f);
    private static Color BOX_BORDER_COLOR = new Color(0.35f, 0.35f, 0.13f);
    private static float MAX_SCROLL_HEIGHT = 100;
    private static float MIN_SCROLL_HEIGHT = 60;
    private static float SCROLLSPEED = 5;
    private static float BORDER_SIZE = 20f;
    private static float SPEED = 1f;

    // Private fields
    private List<Observer> m_Observers;
    private HashSet<Unit> m_SelectedUnits;
    private State m_CurrentState;
    private Vector3 m_MousePos;
    private Rect m_ScreenBorderInverse;

    // Use this for initialization
    void Start () {
        // Handle public constants
        DESELECT_KEY = KeyCode.LeftShift; // TODO make custom binds

        // Handle private fields
        m_Observers = new List<Observer>();
        m_Observers.Add(new UIObserver());
        m_Observers.Add(new GameObserver());
        m_SelectedUnits = new HashSet<Unit>();

        // Rectangle that contains everything EXCEPT the screen border
        m_ScreenBorderInverse = new Rect(BORDER_SIZE, BORDER_SIZE, Screen.width - BORDER_SIZE * 2, Screen.height - BORDER_SIZE);

        m_CurrentState = new SelectedState(this);
    }

    void OnGUI()
    {
        Rect rect = Utils.GetScreenRect(m_MousePos, Input.mousePosition);
        Utils.DrawScreenRect(rect, BOX_INTERIOR_COLOR);
        Utils.DrawScreenRectBorder(rect, 2, BOX_BORDER_COLOR);
    }

    void Update () {
        m_CurrentState.HandleInput();
        m_CurrentState.StateUpdate();
        Scroll();
        EdgePan();
    }

    public void NotifyAll<T>(string invocation, params T[] data)
    {
        foreach (Observer o in m_Observers)
        {
            o.OnNotify(this, invocation, data);
        }
    }

    /// <summary>
    /// Handles scrolling in and out with the mouse wheel.
    /// </summary>
    private void Scroll() { 
        float scroll = Input.GetAxis("Mouse ScrollWheel");
		    if(scroll< 0 && transform.position.y< MAX_SCROLL_HEIGHT)
        {
			    transform.Translate(0, scroll* SCROLLSPEED, scroll * SCROLLSPEED);
		    } else if (scroll > 0 && transform.position.y > MIN_SCROLL_HEIGHT) {
			    transform.Translate(0, scroll* SCROLLSPEED, scroll * SCROLLSPEED);
		    }
    }

    /// <summary>
    /// Handles edge panning, based on the position of the mouse.
    /// </summary>
    private void EdgePan()
    {
        // Is the mouse at the edge of the screen?
        if (!m_ScreenBorderInverse.Contains(m_MousePos))
        {
            Vector2 v = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            v.Normalize();
            v *= SPEED;
            transform.Translate(v.x, 0, v.y, Space.World);
        }
    }

    /// <summary>
    /// Stores the mouse position variable.
    /// </summary>
    /// <param name="newPos"></param>
    private void StoreMousePos(Vector3 newPos)
    {
        m_MousePos = newPos;
    }

    /// <summary>
    /// Deselects all units.
    /// </summary>
    private void DeselectAll()
    {
        // If there's a menu up displaying unit info, close it
        NotifyAll<VoidObject>(UIObserver.CLOSE_ALL);
        NotifyAll<VoidObject>(GameObserver.UNITS_DESELECTED);
    }

    /// <summary>
    /// Handles all state involved with selected units after drawing the 
    /// selection rectangle is completed.
    /// </summary>
    class SelectedState : State
    {
        private CameraController m_CameraController;

        public SelectedState(CameraController controller)
        {
            m_CameraController = controller;
        }

        public void HandleInput()
        {
            // Store the current mouse position.
            m_CameraController.StoreMousePos(Input.mousePosition);

            // Deselect units when the deselect key is pressed
            if (Input.GetKey(DESELECT_KEY))
            {
                m_CameraController.DeselectAll();
            }

            // Starts drawing when left mouse button is pressed by switching
            // to DrawingState
            if (Input.GetMouseButtonDown(0))
            {
                // Ignore user clicking on UI
                if (Utils.MouseIsOverUI())
                {
                    return;
                }
                
                // Start drawing.
                m_CameraController.m_CurrentState = new DrawingState(m_CameraController);
                return;
            }

        }

        public void StateUpdate()
        {
        }


    }

    /// <summary>
    /// State that handles all behavior involving drawing a box, but NOT with
    /// selecting units.
    /// </summary>
    class DrawingState : State
    {
        private CameraController m_CameraController;
        private Camera m_Camera;
        //private HashSet<Unit> m_SelectedUnits;

        public DrawingState(CameraController controller)
        {
            m_CameraController = controller;
            m_Camera = controller.m_Camera;

            //m_SelectedUnits = controller.m_SelectedUnits;
        }

        public void HandleInput()
        {
            // When mouse button is up, switch back to drawing state.
            if (Input.GetMouseButtonUp(0))
            {
                m_CameraController.NotifyAll(GameObserver.UNITS_SELECTED, m_CameraController.m_SelectedUnits);
                m_CameraController.m_CurrentState = new SelectedState(m_CameraController);
                return;
            }

        }

        public void StateUpdate()
        {
            // Take the selection box and highlight all the objects inside
            foreach (Unit s in FindObjectsOfType<Unit>())
            {
                if (IsWithinSelectionBounds(s))
                {
                    s.Highlight();
                    m_CameraController.m_SelectedUnits.Add(s);
                }
                else
                {
                    s.RemoveHighlight();
                    m_CameraController.m_SelectedUnits.Remove(s);
                }
            }
        }

        // Checks to see if a given object is within the area being selected
        private bool IsWithinSelectionBounds(Unit unit)
        {
            Bounds viewportBounds = Utils.GetViewportBounds(m_Camera, m_CameraController.m_MousePos, Input.mousePosition);
            return viewportBounds.Contains(m_Camera.WorldToViewportPoint(unit.transform.position));
        }
    }

}

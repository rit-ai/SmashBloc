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
 * 
 * The two-state behavior is necessary due to the fact that the controller must
 * be able to identify when the user is clicking, clicking and dragging, etc.
 * **/
public class CameraController : MonoBehaviour, Observable {

    // Public constants
    public static KeyCode DESELECT_KEY;

    // Public fields
    public Camera m_Camera;
    public LayerMask Terrain_mask;

    // Private constants
    // Sic: Don't ask me how this works.
    private static Vector3 SCREEN_CENTER = new Vector3(Screen.width, Screen.height);
    private static Color BOX_INTERIOR_COLOR = new Color(0.74f, 0.71f, 0.27f, 0.5f);
    private static Color BOX_BORDER_COLOR = new Color(0.35f, 0.35f, 0.13f);
    private static float MAX_CAMERA_SIZE = 200f;
    private static float MIN_CAMERA_SIZE = 50f;
    private static float SCROLLSPEED = 50f;
    private static float BORDER_SIZE = 10f;
    private static float SPEED = 3f;

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

    /// <summary>
    /// Essential function for drawing the selection box.
    /// </summary>
    void OnGUI()
    {
        Rect rect = Utils.GetScreenRect(m_MousePos, Input.mousePosition);
        Utils.DrawScreenRect(rect, BOX_INTERIOR_COLOR);
        Utils.DrawScreenRectBorder(rect, 2, BOX_BORDER_COLOR);
    }

    void Update () {
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
        float size = m_Camera.orthographicSize;
        size -= scroll * SCROLLSPEED;
        size = Mathf.Max(size, MIN_CAMERA_SIZE);
        size = Mathf.Min(size, MAX_CAMERA_SIZE);
        m_Camera.orthographicSize = size;

    }

    /// <summary>
    /// Handles edge panning, based on the position of the mouse.
    /// </summary>
    private void EdgePan()
    {
        // Is the mouse at the edge of the screen?
        if (!m_ScreenBorderInverse.Contains(m_MousePos))
        {
            Quaternion rotation = m_Camera.transform.rotation;
            //Vector2 direction = new Vector2(Input.mousePosition.x + Screen.width / 2, Input.mousePosition.y + Screen.height / 2);
            Vector3 direction = SCREEN_CENTER - m_MousePos;
            direction.Normalize();
            direction *= SPEED;
            transform.Translate(direction.x, 0, direction.y, Space.World);
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
    private class SelectedState : State
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
            HandleInput();
        }


    }

    /// <summary>
    /// State that handles all behavior involving drawing a box, but NOT with
    /// selecting units.
    /// </summary>
    private class DrawingState : State
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

        /// <summary>
        /// Determines if the state needs to be switched (when the user stops 
        /// dragging the pointer).
        /// </summary>
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

        /// <summary>
        /// Updates the units and the selected units list based on whether or 
        /// not they're in the selection box.
        /// </summary>
        public void StateUpdate()
        {
            HandleInput();

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

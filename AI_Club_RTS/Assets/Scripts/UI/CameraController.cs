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
public class CameraController : MonoBehaviour {

    // Public constants
    public static KeyCode DESELECT_KEY;

    // Private constants
    private static Color BOX_INTERIOR_COLOR = new Color(0.74f, 0.71f, 0.27f, 0.5f);
    private static Color BOX_BORDER_COLOR = new Color(0.35f, 0.35f, 0.13f);
    private static float MAX_SCROLL_HEIGHT = 100;
    private static float MIN_SCROLL_HEIGHT = 60;
    private static float SCROLLSPEED = 5;
    private static float BORDER_SIZE = 20f;
    private static float SPEED = 1f;

    // Public fields
    public Camera m_Camera;

    // Private fields
    private State m_CurrentState;
    public LayerMask UI_mask;
    private Vector3 m_MousePos;
    private Rect m_ScreenBorderInverse;
    private List<Unit> m_SelectedUnits;

    // Use this for initialization
    public void Start () {
        // Handle public constants
        DESELECT_KEY = KeyCode.LeftShift; // TODO make custom binds

        // Handle private fields
        // Rectangle that contains everything EXCEPT the screen border
        m_ScreenBorderInverse = new Rect(BORDER_SIZE, BORDER_SIZE, Screen.width - BORDER_SIZE * 2, Screen.height - BORDER_SIZE);

        m_SelectedUnits = new List<Unit>();
        m_CurrentState = new SelectedState(this);
        UI_mask = ~UI_mask; // Invert, since we want our raycasts to look for UI elements.
    }

    void OnGUI()
    {
        Rect rect = Utils.GetScreenRect(m_MousePos, Input.mousePosition);
        Utils.DrawScreenRect(rect, BOX_INTERIOR_COLOR);
        Utils.DrawScreenRectBorder(rect, 2, BOX_BORDER_COLOR);
    }

    public void Update () {
        m_CurrentState.HandleInput();
        m_CurrentState.StateUpdate();
        Scroll();
        EdgePan();
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
        FindObjectOfType<Unit>().NotifyObservers(MenuObserver.SUPPRESS_UNIT_DATA);
        // Remove highlight from all units
        foreach (Unit s in m_SelectedUnits)
        {
            s.RemoveHighlight();
        }
        // Clear the list of selected units
        m_SelectedUnits.Clear();
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
            // Deselect units when the deselect key is pressed
            if (Input.GetKey(DESELECT_KEY))
            {
                m_CameraController.DeselectAll();
            }

            // Starts drawing when left mouse button is pressed by switching
            // to DrawingState
            if (Input.GetMouseButtonDown(0))
            {
                // Ignore input if the user clicks on a UI element FIXME
                /*
                Ray ray = Camera.main.ScreenPointToRay(m_CameraController.m_MousePos);
                RaycastHit hit = new RaycastHit();
                Physics.RaycastAll(ray, 10000f, m_CameraController.UI_mask);
                if (Physics.Raycast(ray, out hit, m_CameraController.UI_mask))
                {
                    return;
                }
                */
                // Start drawing.
                m_CameraController.m_CurrentState = new DrawingState(m_CameraController);
                return;
            }

            // Store the current mouse position.
            m_CameraController.StoreMousePos(Input.mousePosition);

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

        private List<Unit> selectedUnits;

        public DrawingState(CameraController controller)
        {
            m_CameraController = controller;
            m_Camera = controller.m_Camera;

            selectedUnits = controller.m_SelectedUnits;
        }

        public void HandleInput()
        {
            // When mouse button is up, switch back to drawing state.
            if (Input.GetMouseButtonUp(0))
            {
                // Handle for if a unit is solo selected
                if (selectedUnits.Count == 1)
                {
                    selectedUnits[0].SoloSelected();
                }

                m_CameraController.m_CurrentState = new SelectedState(m_CameraController);
                return;
            }

            foreach (Unit s in selectedUnits)
            {
                s.RemoveHighlight();
            }
            selectedUnits.Clear();

            // Take the selection box and highlight all the objects inside
            foreach (Unit s in GameObject.FindObjectsOfType<Unit>())
            {
                if (IsWithinSelectionBounds(s))
                {
                    s.Highlight();
                    selectedUnits.Add(s);
                }
            }

            // If the selection box is empty, deselect everyone
            if (selectedUnits.Count == 0)
            {
                m_CameraController.DeselectAll();
            }
        }

        public void StateUpdate()
        {
        }

        // Checks to see if a given object is within the area being selected
        private bool IsWithinSelectionBounds(Unit unit)
        {
            Bounds viewportBounds = Utils.GetViewportBounds(m_Camera, m_CameraController.m_MousePos, Input.mousePosition);
            return viewportBounds.Contains(m_Camera.WorldToViewportPoint(unit.transform.position));
        }
    }

}

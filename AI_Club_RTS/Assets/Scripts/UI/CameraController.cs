using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Public constants
    public static KeyCode DESELECT_KEY;

    // Private constants
    private static float MAX_SCROLL_HEIGHT = 100;
    private static float MIN_SCROLL_HEIGHT = 60;
    private static float SCROLLSPEED = 5;
    private static float BORDER_SIZE = 20f;
    private static float SPEED = 1f;

    // Public fields
    public Camera m_Camera;

    // Private fields
    private State m_CurrentState;
    private Vector3 m_MousePos;
    private Rect screenBorderInverse;

    // Use this for initialization
    public void Start () {
        // Handle public constants
        DESELECT_KEY = KeyCode.LeftShift; // TODO make custom binds

        // Handle private fields
        // Rectangle that contains everything EXCEPT the screen border
        screenBorderInverse = new Rect(BORDER_SIZE, BORDER_SIZE, Screen.width - BORDER_SIZE * 2, Screen.height - BORDER_SIZE);

        m_CurrentState = new SelectionState(this);
    }

    void OnGUI()
    {
        Rect rect = Utils.GetScreenRect(m_MousePos, Input.mousePosition);
        Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
        Utils.DrawScreenRectBorder(rect, 2, new Color(.8f, .0f, 0.95f));
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
        if (!screenBorderInverse.Contains(m_MousePos))
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
        foreach (Unit s in GameObject.FindObjectsOfType<Unit>())
        {
            s.Deselect();
        }
    }

    /// <summary>
    /// Handles all state involved with selected units after drawing the 
    /// selection rectangle is completed.
    /// </summary>
    class SelectionState : State
    {
        private CameraController m_CameraController;

        public SelectionState(CameraController controller)
        {
            m_CameraController = controller;
        }

        public override void HandleInput()
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
                m_CameraController.m_CurrentState = new DrawingState(m_CameraController);
                return;
            }

            m_CameraController.StoreMousePos(Input.mousePosition);
        }

        public override void StateUpdate()
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

        public DrawingState(CameraController controller)
        {
            m_CameraController = controller;
            m_Camera = controller.m_Camera;
        }

        public override void HandleInput()
        {
            // When mouse button is up, switch back to drawing state.
            if (Input.GetMouseButtonUp(0))
            {
                m_CameraController.m_CurrentState = new SelectionState(m_CameraController);
                return;
            }

            // Take the selection box and highlight all the objects inside
            foreach (Unit s in GameObject.FindObjectsOfType<Unit>())
               {
                s.Deselect();
                if (IsWithinSelectionBounds(s))
                {
                    s.Select();
                }
            }
        }

        public override void StateUpdate()
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

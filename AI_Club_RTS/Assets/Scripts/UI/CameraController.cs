using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	
	private Vector3 _mousePos;
	private static float MAX_SCROLL_HEIGHT = 100;
	private static float MIN_SCROLL_HEIGHT = 60;
	private static float SCROLLSPEED = 5;

    public static KeyCode DESELECT_KEY;

    public Camera m_Camera;

    private State m_CurrentState;
    private Vector3 m_MousePos;

    // Use this for initialization
    public void Start () {
        DESELECT_KEY = KeyCode.LeftShift; // TODO make custom binds

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

        private Rect recdown, recup, recleft, recright;
        private static float GUI_SIZE;
        private static float SPEED;

        public SelectionState(CameraController controller)
        {
            m_CameraController = controller;

            GUI_SIZE = 75;
            SPEED = 0.5f;

            recdown = new Rect(0, 0, Screen.width, GUI_SIZE);
            recup = new Rect(0, Screen.height - GUI_SIZE, Screen.width, GUI_SIZE);
            recleft = new Rect(0, 0, GUI_SIZE, Screen.height);
            recright = new Rect(Screen.width - GUI_SIZE, 0, GUI_SIZE, Screen.height);
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

            // Edge panning functionality FIXME
            recdown = new Rect(0, 0, Screen.width, GUI_SIZE);
            recup = new Rect(0, Screen.height - GUI_SIZE, Screen.width, GUI_SIZE);
            recleft = new Rect(0, 0, GUI_SIZE, Screen.height);
            recright = new Rect(Screen.width - GUI_SIZE, 0, GUI_SIZE, Screen.height);
            if (recdown.Contains(Input.mousePosition) ||
                recup.Contains(Input.mousePosition) ||
                recleft.Contains(Input.mousePosition) ||
                recright.Contains(Input.mousePosition))
            {
                Vector2 v = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
                v.Normalize();
                v *= SPEED;
                //m_CameraController.transform.Translate(v.x, 0, v.y, Space.World);
            }
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

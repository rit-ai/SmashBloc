using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This class handles logic involving the movement of the camera, and player 
 * interaction with the world space (NOT menus). Rather than handling what it 
 * MEANS when a Player clicks and drags over units, CameraController just 
 * reports it and leaves the rest to its Observers.
 * 
 * This lets the CameraController focus on its two key responsibilities: the
 * Camera, and the Controls.
 * **/
public class CameraController : MonoBehaviour, IObservable {

    // Public constants
    public static KeyCode DESELECT_KEY;

    // Public fields
    public KeyCode m_PauseButton;
    // vv "Arrow Keys" vv
    public KeyCode m_MoveCameraForward;
    public KeyCode m_MoveCameraLeft;
    public KeyCode m_MoveCameraRight;
    public KeyCode m_MoveCameraBack;

    // Private constants
    private readonly Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
    private readonly Color BOX_INTERIOR_COLOR = new Color(0.74f, 0.71f, 0.27f, 0.5f);
    private readonly Color BOX_BORDER_COLOR = new Color(0.35f, 0.35f, 0.13f);
    private const string CAMERA_RIG_TAG = "CameraRig";
    private const float CAMERA_BEHIND_OFFSET = 200f;
    private const float MAX_CAMERA_SIZE = 200f;
    private const float MIN_CAMERA_SIZE = 50f;
    private const float SCROLLSPEED = 50f;
    private const float BORDER_SIZE = 10f;
    private const float SPEED = 3f;

    // Private fields
    private Camera m_Camera;
    private Transform m_CameraRig;
    private List<IObserver> m_Observers;
    private HashSet<Unit> m_SelectedUnits;
    private State m_CurrentState;
    private Vector3 m_MousePos;
    private Rect m_ScreenBorderInverse;

    private Vector3 direction;
    private bool arrowMoving = false;

    // These critical variables must be assigned before anything else
    private void Awake()
    {
        m_Camera = Camera.main;
        m_CameraRig = GameObject.FindGameObjectWithTag(CAMERA_RIG_TAG).transform;
    }

    // Use this for initialization
    void Start () {
        // Handle public constants
        DESELECT_KEY = KeyCode.LeftShift; // TODO make custom binds

        // Handle private fields
        m_Observers = new List<IObserver>
        {
            gameObject.AddComponent<UIObserver>(),
            gameObject.AddComponent<GameObserver>()
        };
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
        ArrowMove();
        EdgePan();
        Pause();
    }

    public void NotifyAll(Invocation invoke, params object[] data)
    {
        foreach (IObserver o in m_Observers)
        {
            o.OnNotify(this, invoke, data);
        }
    }

    /// <summary>
    /// Centers the camera behind a position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="target"></param>
    public void CenterCameraBehindPosition(Vector3 position, Vector3 target)
    {
        // Move the camera over and behind the position
        Vector3 dest = position;
        // Set values so that the orientation of the camera is unchanged
        dest.y = m_CameraRig.transform.position.y;

        m_CameraRig.transform.position = dest;
        // Rotate the camera toward target (TODO: right now is just map center)
        Quaternion rotation = Quaternion.LookRotation(target - dest);
        rotation.x = 0; rotation.z = 0; // x and z aren't relevant

        // Apply an offset so that we end up behind the target
        dest -= target;
        dest.y = 0;
        dest = Vector3.ClampMagnitude(dest, CAMERA_BEHIND_OFFSET);
        m_CameraRig.transform.Translate(dest);
        m_CameraRig.transform.rotation = rotation;
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
    /// Moves the camera based on which "arrow keys" are pressed.
    /// </summary>
    private void ArrowMove()
    {
        arrowMoving = false;
        direction = Vector3.zero;

        if (Input.GetKey(m_MoveCameraForward))
        {
            direction += Vector3.forward;
            arrowMoving = true;
        }

        if (Input.GetKey(m_MoveCameraLeft))
        {
            direction += Vector3.left;
            arrowMoving = true;
        }

        if (Input.GetKey(m_MoveCameraRight))
        {
            direction += Vector3.right;
            arrowMoving = true;
        }

        if (Input.GetKey(m_MoveCameraBack))
        {
            direction += Vector3.back;
            arrowMoving = true;
        }

        MoveCamera(direction);
    }

    /// <summary>
    /// Handles edge panning, based on the position of the mouse.
    /// 
    /// FIXME: Right now edge panning up or down breaks if the rotation of the
    /// camera is changed.
    /// </summary>
    private void EdgePan()
    {
        // Edge panning is disabled while using arrow keys to move.
        if (arrowMoving) { return; }

        // Is the mouse at the edge of the screen?
        if (!m_ScreenBorderInverse.Contains(m_MousePos))
        {
            direction = SCREEN_CENTER - m_MousePos;
            direction.x = -direction.x;
            direction.z = -direction.y;
            MoveCamera(direction);
        }
    }

    /// <summary>
    /// Moves the camera in a direction relative to the rotation of the 
    /// CameraRig. Since the local axis of the transform is importnat, we 
    /// CANNOT use the Camera's primary transform (as it is pointed at the 
    /// ground--if we moved it forward, the view would go into the ground).
    /// </summary>
    private void MoveCamera(Vector3 direction)
    {
        direction.y = 0f;
        direction.Normalize();
        direction *= SPEED;
        m_CameraRig.transform.Translate(direction, Space.Self);
    }

    /// <summary>
    /// Pauses the game when the pause button is pressed.
    /// </summary>
    private void Pause()
    {
        if (Input.GetKeyDown(m_PauseButton))
        {
            NotifyAll(Invocation.TOGGLE_PAUSE);
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
        NotifyAll(Invocation.CLOSE_ALL);
        NotifyAll(Invocation.UNITS_DESELECTED);
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

                // Ignore user clicking on anything but the RTS_terrain
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag(RTS_Terrain.TERRAIN_TAG))
                    {
                        // Start drawing.
                        m_CameraController.m_CurrentState = new DrawingState(m_CameraController);
                        return;
                    }

                }

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
                m_CameraController.NotifyAll(Invocation.UNITS_SELECTED, m_CameraController.m_SelectedUnits);
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

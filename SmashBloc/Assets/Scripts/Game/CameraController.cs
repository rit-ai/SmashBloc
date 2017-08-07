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
    public KeyCode pause;
    public KeyCode escMenu;
    public KeyCode deselect;
    // vv "Arrow Keys" vv
    public KeyCode moveForward;
    public KeyCode moveLeft;
    public KeyCode moveBack;
    public KeyCode moveRight;

    // Private constants
    private readonly Vector3 DEFAULT_CAMERA_LOC = new Vector3(0f, 400f, 0f);
    private readonly Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
    private readonly Color BOX_INTERIOR_COLOR = new Color(0.74f, 0.71f, 0.27f, 0.5f);
    private readonly Color BOX_BORDER_COLOR = new Color(0.35f, 0.35f, 0.13f);
    private const string CAMERA_RIG_TAG = "CameraRig";
    private const float CAMERA_BEHIND_OFFSET = 500f;
    private const float MAX_CAMERA_SIZE = 300f;
    private const float MIN_CAMERA_SIZE = 50f;
    private const float SCROLLSPEED = 50f;
    private const float BORDER_SIZE = 10f;
    private const float SPEED = 3f;

    // Private fields
    private Camera cam;
    private Transform camRig;
    private List<IObserver> observers;
    private List<MobileUnit> selectedUnits;
    private State state;
    private Vector3 mousePos;
    private Rect screenBorderInverse;

    private Vector3 direction;
    private bool arrowMoving = false;

    // These critical variables must be assigned before anything else
    private void Awake()
    {
        cam = Camera.main;
        camRig = GameObject.FindGameObjectWithTag(CAMERA_RIG_TAG).transform;
    }

    // Use this for initialization
    void Start () {
        // Handle private fields
        observers = new List<IObserver>
        {
            Toolbox.GameObserver,
            Toolbox.UIObserver
        };
        selectedUnits = new List<MobileUnit>();

        // Rectangle that contains everything EXCEPT the screen border
        screenBorderInverse = new Rect(BORDER_SIZE, BORDER_SIZE, Screen.width - BORDER_SIZE * 2, Screen.height - BORDER_SIZE);

        state = new SelectedState(this);
    }

    /// <summary>
    /// Essential function for drawing the selection box.
    /// </summary>
    void OnGUI()
    {
        Rect rect = Utils.GetScreenRect(mousePos, Input.mousePosition);
        Utils.DrawScreenRect(rect, BOX_INTERIOR_COLOR);
        Utils.DrawScreenRectBorder(rect, 2, BOX_BORDER_COLOR);
    }

    void Update () {
        state.StateUpdate();
        CheckForInput();
    }

    public void NotifyAll(Invocation invoke, params object[] data)
    {
        foreach (IObserver o in observers)
        {
            o.OnNotify(this, invoke, data);
        }
    }

    /// <summary>
    /// Centers the camera behind a target.
    /// 
    /// TODO right now the camera is always facing the center of the map, but
    /// that could be changed if needed.
    /// </summary>
    /// <param name="target">What to look at.</param>
    public void CenterCameraBehindPosition(Vector3 target)
    {
        // Reset camera to center of map
        camRig.transform.position = DEFAULT_CAMERA_LOC;
        // Get rotation looking away from target
        Quaternion rotation = Quaternion.LookRotation(-target);
        rotation.x = 0; rotation.z = 0; // We don't care about these values

        // Get opposite of target and extend it to exactly the desired length
        Vector3 position = target;
        position.Normalize();
        position *= CAMERA_BEHIND_OFFSET;
        position.y = DEFAULT_CAMERA_LOC.y; // ignore y component of dest

        camRig.transform.SetPositionAndRotation(position, rotation);
    }

    /// <summary>
    /// Checks for input on relevant keys.
    /// </summary>
    private void CheckForInput()
    {
        Scroll();
        ArrowMove();
        EdgePan();
        Pause();
        PauseMenu();
    }

    /// <summary>
    /// Handles scrolling in and out with the mouse wheel.
    /// </summary>
    private void Scroll() { 
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float size = cam.orthographicSize;
        size -= scroll * SCROLLSPEED;
        size = Mathf.Max(size, MIN_CAMERA_SIZE);
        size = Mathf.Min(size, MAX_CAMERA_SIZE);
        cam.orthographicSize = size;
    }

    /// <summary>
    /// Moves the camera based on which "arrow keys" are pressed.
    /// </summary>
    private void ArrowMove()
    {
        arrowMoving = false;
        direction = Vector3.zero;

        if (Input.GetKey(moveForward))
        {
            direction += Vector3.forward;
            arrowMoving = true;
        }

        if (Input.GetKey(moveLeft))
        {
            direction += Vector3.left;
            arrowMoving = true;
        }

        if (Input.GetKey(moveRight))
        {
            direction += Vector3.right;
            arrowMoving = true;
        }

        if (Input.GetKey(moveBack))
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
        if (!screenBorderInverse.Contains(mousePos))
        {
            direction = SCREEN_CENTER - mousePos;
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
        camRig.transform.Translate(direction, Space.Self);
    }

    /// <summary>
    /// Pauses the game when the pause button is pressed.
    /// </summary>
    private void Pause()
    {
        if (Input.GetKeyDown(pause))
        {
            NotifyAll(Invocation.TOGGLE_PAUSE);
        }
    }
    
    /// <summary>
    /// Expresses that the button to open the pause menu has been pressed.
    /// </summary>
    private void PauseMenu()
    {
        if (Input.GetKeyDown(escMenu))
        {
            NotifyAll(Invocation.PAUSE_AND_LOCK);
        }
    }

    /// <summary>
    /// Stores the mouse position variable.
    /// </summary>
    /// <param name="newPos"></param>
    private void StoreMousePos(Vector3 newPos)
    {
        mousePos = newPos;
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
                        m_CameraController.state = new DrawingState(m_CameraController);
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

        public DrawingState(CameraController controller)
        {
            m_CameraController = controller;
            m_Camera = controller.cam;
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
                m_CameraController.NotifyAll(Invocation.UNITS_SELECTED, m_CameraController.selectedUnits);
                m_CameraController.state = new SelectedState(m_CameraController);
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
            foreach (MobileUnit s in FindObjectsOfType<MobileUnit>())
            {
                if (IsWithinSelectionBounds(s))
                {
                    s.Highlight();
                    m_CameraController.selectedUnits.Add(s);
                }
                else
                {
                    s.RemoveHighlight();
                    m_CameraController.selectedUnits.Remove(s);
                }
            }
        }

        // Checks to see if a given object is within the area being selected
        private bool IsWithinSelectionBounds(MobileUnit unit)
        {
            Bounds viewportBounds = Utils.GetViewportBounds(m_Camera, m_CameraController.mousePos, Input.mousePosition);
            return viewportBounds.Contains(m_Camera.WorldToViewportPoint(unit.transform.position));
        }
    }

}

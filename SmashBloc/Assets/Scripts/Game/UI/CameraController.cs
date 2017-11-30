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
public class CameraController : MonoBehaviour, IObservable
{
    // **         //
    // * FIELDS * //
    //         ** //

    public Transform camRig;

    public KeyCode pause;
    public KeyCode escMenu;
    public KeyCode deselect;
    // vv "Arrow Keys" vv
    public KeyCode moveForward;
    public KeyCode moveLeft;
    public KeyCode moveBack;
    public KeyCode moveRight;

    private readonly Vector3 DEFAULT_CAMERA_LOC = new Vector3(0f, 400f, 0f);
    private readonly Color BOX_INTERIOR_COLOR = new Color(0.74f, 0.71f, 0.27f, 0.5f);
    private readonly Color BOX_BORDER_COLOR = new Color(0.35f, 0.35f, 0.13f);
    private const string CAMERA_RIG_TAG = "CameraRig";
    private const float CAMERA_BEHIND_OFFSET = 700f;
    private const float MAX_CAMERA_SIZE = 300f;
    private const float MIN_CAMERA_SIZE = 50f;
    private const float SCROLLSPEED = 50f;
    private const float BORDER_SIZE = 10f;
    private const float SPEED = 4f;

    private IState state;
    private List<IObserver> observers;
    private List<MobileUnit> selectedUnits;
    private Camera cam;
    private Rect screenBorderInverse;
    private Vector3 screenCenter;
    private Vector3 mousePos;
    private Vector3 direction;
    private bool arrowMoving = false;

    // **          //
    // * METHODS * //
    //          ** // 

    public void NotifyAll(Invocation invoke, params object[] data)
    {
        foreach (IObserver o in observers)
        {
            if (o == null) { continue; }
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
    /// Responsible for initialization.
    /// </summary>
    void Start()
    {
        observers = new List<IObserver>
        {
            Toolbox.GameObserver,
            Toolbox.UIObserver
        };
        selectedUnits = new List<MobileUnit>();

        cam = Camera.main;
        //camRig = GameObject.FindGameObjectWithTag(CAMERA_RIG_TAG).transform;

        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
        // Rectangle that contains everything EXCEPT the screen border
        screenBorderInverse = new Rect(BORDER_SIZE, BORDER_SIZE, Screen.width - BORDER_SIZE * 2, Screen.height - BORDER_SIZE * 4);

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

    /// <summary>
    /// Handles input.
    /// </summary>
    void Update()
    {
        state.StateUpdate();
        CheckForInput();
    }

    /// <summary>
    /// Checks for input on relevant keys.
    /// </summary>
    private void CheckForInput()
    {
        Scroll();
        ArrowMove();
        EdgePan();

        if (Input.GetKeyDown(pause))
        {
            NotifyAll(Invocation.TOGGLE_PAUSE);
        }

        if (Input.GetKeyDown(escMenu))
        {
            NotifyAll(Invocation.PAUSE_AND_LOCK);
        }
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
            direction = screenCenter - mousePos;
            direction.x = -direction.x;
            direction.z = -direction.y;
            MoveCamera(direction);
        }
    }

    /// <summary>
    /// Moves the camera in a direction relative to the rotation of the 
    /// CameraRig. Since the local axis of the transform is importnat, we 
    /// CANNOT use the main Camera's transform (as it is pointed at the 
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

    /*
     * Handles all state involved with selected units after drawing the
     * selection rectangle is completed.
     * **/
    private class SelectedState : IState
    {
        // **         //
        // * FIELDS * //
        //         ** //

        private CameraController controller;

        // **              //
        // * CONSTRUCTOR * //
        //              ** // 

        public SelectedState(CameraController controller)
        {
            this.controller = controller;
        }

        // **          //
        // * METHODS * //
        //          ** // 

        public void StateUpdate()
        {
            // Store the current mouse position.
            controller.StoreMousePos(Input.mousePosition);

            // Deselect units when the deselect key is pressed
            if (Input.GetKey(controller.deselect))
            {
                controller.DeselectAll();
            }

            // Check to see if the user wants to draw a selection box
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
                        controller.state = new DrawingState(controller);
                        return;
                    }

                }

            }
        }


    }

    /// <summary>
    /// State that handles all behavior involving drawing a box, but NOT with
    /// selecting units.
    /// </summary>
    private class DrawingState : IState
    {
        // **         //
        // * FIELDS * //
        //         ** //

        private CameraController controller;
        private Camera cam;

        // **              //
        // * CONSTRUCTOR * //
        //              ** // 

        public DrawingState(CameraController controller)
        {
            this.controller = controller;
            cam = controller.cam;
        }

        // **          //
        // * METHODS * //
        //          ** // 

        /// <summary>
        /// Determines if the state needs to be switched (when the user stops 
        /// dragging the pointer).
        /// </summary>
        public void HandleInput()
        {
            // When mouse button is up, switch back to drawing state.
            if (Input.GetMouseButtonUp(0))
            {
                controller.NotifyAll(Invocation.UNITS_SELECTED, controller.selectedUnits);
                controller.state = new SelectedState(controller);
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
                    controller.selectedUnits.Add(s);
                }
                else
                {
                    s.RemoveHighlight();
                    controller.selectedUnits.Remove(s);
                }
            }
        }

        /// <summary>
        /// Checks to see if a given unit is within the area being selected.
        /// </summary>
        private bool IsWithinSelectionBounds(MobileUnit unit)
        {
            Bounds viewportBounds = Utils.GetViewportBounds(cam, controller.mousePos, Input.mousePosition);
            return viewportBounds.Contains(cam.WorldToViewportPoint(unit.transform.position));
        }
    }

}

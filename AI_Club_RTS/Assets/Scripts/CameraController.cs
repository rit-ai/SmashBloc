using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private bool isSelecting;
	private List<GameObject> selectedUnits;  // List of current selected units. "Selectable Unit" can be changed to any class that you want to select
	private Rect recdown, recup, recleft, recright;
	private Vector3 mousePos;
	private Camera camera;
	private float speed;
	private float scrollSpeed;
	private float GUISize;

	// Use this for initialization
	void Start () {
		recdown = new Rect (0, 0, Screen.width, GUISize);
		recup = new Rect (0, Screen.height - GUISize, Screen.width, GUISize);
		recleft = new Rect (0, 0, GUISize, Screen.height);
		recright = new Rect (Screen.width - GUISize, 0, GUISize, Screen.height);
		selectedUnits = new List<GameObject>();
		camera = Camera.main;
		isSelecting = false;
		speed = 0.5f;
		scrollSpeed = 5;
		GUISize = 75;
	}
		
	void Update () {

		// Saves the mouse location when left mouse is down
		if(Input.GetMouseButtonDown(0)) {
			isSelecting = true;
			mousePos = Input.mousePosition;
		}

		// Ends selection when left mouse button is released
		if(Input.GetMouseButtonUp(0)) {
			// Gets all the units that can be selected 
			if(!Input.GetKey(KeyCode.LeftShift)) {
				Debug.Log("list cleared, components deselected");
				selectedUnits.Clear();
			}
			foreach(GameObject s in GameObject.FindGameObjectsWithTag("Unit")) {
				if(IsWithinSelectionBounds(s.gameObject) && !selectedUnits.Contains(s)) {
					selectedUnits.Add(s);
					Debug.Log("added");
				}
			}

			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			// Click to add an object
			if(Physics.Raycast(ray, out hit)) {
				if(hit.transform.tag == "Unit" && !selectedUnits.Contains(hit.transform.gameObject)) {
					selectedUnits.Add(hit.transform.gameObject);
					Debug.Log("added");
				}
			}
			isSelecting = false;
		}

		// Camera movement
		if(!isSelecting){
			recdown = new Rect (0, 0, Screen.width, GUISize);
			recup = new Rect (0, Screen.height - GUISize, Screen.width, GUISize);
			recleft = new Rect (0, 0, GUISize, Screen.height);
			recright = new Rect (Screen.width - GUISize, 0, GUISize, Screen.height);
			if (recdown.Contains (Input.mousePosition) ||
				recup.Contains (Input.mousePosition) ||
				recleft.Contains (Input.mousePosition) ||
				recright.Contains (Input.mousePosition))
			{
				Vector2 v = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
				v.Normalize();
				v *= speed;
				transform.Translate(v.x, 0, v.y, Space.World);
			}
		}
		// Scroll in or out with the scroll wheel
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if(scroll < 0 && transform.position.y < 15){
			transform.Translate(0, scroll * scrollSpeed, scroll * scrollSpeed);
		} else if (scroll > 0 && transform.position.y > 10) {
			transform.Translate(0, scroll * scrollSpeed, scroll * scrollSpeed);
		} 
	}

	void OnGUI() {
		// Calls functions to draw the selection box
		if(isSelecting) {
			Rect rect = Utils.GetScreenRect(mousePos, Input.mousePosition);
			Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
			Utils.DrawScreenRectBorder(rect, 2, new Color(.8f, .0f, 0.95f));
		}

	}

	// Checks to see if a given object is within the area being selected
	public bool IsWithinSelectionBounds( GameObject gameObject )
	{
		if( !isSelecting ){
			return false;
		}

		camera = Camera.main;
		Bounds viewportBounds = Utils.GetViewportBounds( camera, mousePos, Input.mousePosition );
		return viewportBounds.Contains( camera.WorldToViewportPoint( gameObject.transform.position ) );
	}
}

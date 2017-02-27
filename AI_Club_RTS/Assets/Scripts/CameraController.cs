using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private bool isSelecting = false;

	private List<GameObject> selectedUnits;  // List of current selected units. "Selectable Unit" can be changed to any class that you want to select
	private Vector3 mousePos;
	private float speed = 0.5f;
	private float GUISize = 75;

	// Use this for initialization
	void Start () {
		selectedUnits = new List<GameObject>();
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
			if(! Input.GetKey(KeyCode.LeftShift)) {
				Debug.Log("list cleared, components deselected");
				selectedUnits.Clear();
			}
			foreach(GameObject s in GameObject.FindGameObjectsWithTag("Unit")) {
				if(IsWithinSelectionBounds(s.gameObject) && !selectedUnits.Contains(s)) {
					selectedUnits.Add(s);
					Debug.Log("added");
				}
			}
			isSelecting = false;
		}

		// Camera movement
		Rect recdown = new Rect (0, 0, Screen.width, GUISize);

		Rect recup = new Rect (0, Screen.height - GUISize, Screen.width, GUISize);

		Rect recleft = new Rect (0, 0, GUISize, Screen.height);

		Rect recright = new Rect (Screen.width - GUISize, 0, GUISize, Screen.height);

		if (recdown.Contains (Input.mousePosition) ||
			recup.Contains (Input.mousePosition) ||
			recleft.Contains (Input.mousePosition) ||
			recright.Contains (Input.mousePosition))
		{
			//Vector2 middle = new Vector2(Screen.width/2, Screen.height/2);
			Vector2 v = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2);
			v.Normalize();
			v *= speed;
			transform.Translate(v.x, 0, v.y, Space.World);
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

		Camera camera = Camera.main;
		Bounds viewportBounds = Utils.GetViewportBounds( camera, mousePos, Input.mousePosition );
		return viewportBounds.Contains( camera.WorldToViewportPoint( gameObject.transform.position ) );
	}
}

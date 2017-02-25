using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour {

	bool isSelecting = false;

	List<SelectableUnit> selectedUnits;  // List of current selected units. "Selectable Unit" can be changed to any class that you want to select
	Vector3 mousePos;
//	List<SelectableUnit> selectedUnits;

	// Use this for initialization
	void Start () {
		selectedUnits = new List<SelectableUnit>();
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
				Debug.Log("new list, components deselected");
				selectedUnits = new List<SelectableUnit>();
			}
			foreach(SelectableUnit s in FindObjectsOfType<SelectableUnit>()) {
				if(IsWithinSelectionBounds(s.gameObject) && !selectedUnits.Contains(s)) {
					selectedUnits.Add(s);
					Debug.Log("added");
				}
			}
			isSelecting = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour {

	bool isSelecting = false;
	Vector3 mousePos;
	List<SelectableUnit> selectedUnits;

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		if(selectedUnits != null) {
			//Debug.Log(selectedUnits.Count.ToString());
		}
		// Saves the mouse location when left mouse is down
		if(Input.GetMouseButtonDown(0)) {
			isSelecting = true;
			mousePos = Input.mousePosition;
		}
		// Ends selection when left mouse button is released

		// If we let go of the left mouse button, finish selection
		if(Input.GetMouseButtonUp(0)) {
			// Gets all the units that can be selected 
			selectedUnits = new List<SelectableUnit>();
			foreach(var s in FindObjectsOfType<SelectableUnit>()) {
				if(IsWithinSelectionBounds(s.gameObject)) {
					selectedUnits.Add(s);
				}
			}
			isSelecting = false;
		}
	}

	void OnGUI() {
		// Calls functions to draw the selection box
		if(isSelecting) {
			var rect = Utils.GetScreenRect(mousePos, Input.mousePosition);
			Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
			Utils.DrawScreenRectBorder(rect, 2, new Color(.8f, .0f, 0.95f));
		}

	}

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

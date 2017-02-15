using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour {

	bool isSelecting = false;
	Vector3 mousePos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Saves the mouse location when left mouse is down
		if(Input.GetMouseButtonDown(0)) {
			isSelecting = true;
			mousePos = Input.mousePosition;
		}
		// Ends selection when left mouse button is released
		if(Input.GetMouseButtonUp(0)) {
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

	public bool IsWithinSelectionBounds(GameObject gameObject) {
		if(!isSelecting) {
			return false;
		}

		Camera camera = Camera.main;
		Bounds viewPortBounds = Utils.GetViewportBounds(camera, mousePos, Input.mousePosition);
		return viewPortBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
	}
}

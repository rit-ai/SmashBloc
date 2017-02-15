using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class used to draw rectangles on the screen
public class Utils : MonoBehaviour {
	static Texture2D whiteTexture;

	public static Texture2D WhiteTexture {
		get {
			if(whiteTexture == null) {
				whiteTexture = new Texture2D(1,1);
				whiteTexture.SetPixel(0, 0, Color.white);
				whiteTexture.Apply();
			}
			return whiteTexture;
		}
	}

	// Draws a rectangle with the passed in color to the screen
	public static void DrawScreenRect(Rect rect, Color color) {
		GUI.color = color;
		GUI.DrawTexture(rect, WhiteTexture);
		GUI.color = Color.white;
	}

	// Creates a border of a rectangle with color, color and with a border thickness, thickness
	public static void DrawScreenRectBorder( Rect rect, float thickness, Color color )
	{
		// Top
		Utils.DrawScreenRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
		// Left
		Utils.DrawScreenRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
		// Right
		Utils.DrawScreenRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color);
		// Bottom
		Utils.DrawScreenRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
	}

	// Returns a rectangle based on the 2 input screen positions
	public static Rect GetScreenRect(Vector3 screenPos1, Vector3 screenPos2){
		// Move origin from bottom left to top left
		screenPos1.y = Screen.height - screenPos1.y;
		screenPos2.y = Screen.height - screenPos2.y;
		// Calculate corners
		var topLeft = Vector3.Min(screenPos1, screenPos2);
		var bottomRight = Vector3.Max(screenPos1, screenPos2);
		// Create and return rectangle
		return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
	}

	// Creates a bound, based on where the mouse dragged, that will contain the object top be selected
	public static Bounds GetViewportBounds( Camera camera, Vector3 screenPosition1, Vector3 screenPosition2 )
	{
		var v1 = camera.ScreenToViewportPoint( screenPosition1 );
		var v2 = camera.ScreenToViewportPoint( screenPosition2 );
		var min = Vector3.Min( v1, v2 );
		var max = Vector3.Max( v1, v2 );
		min.z = camera.nearClipPlane;
		max.z = camera.farClipPlane;
		//min.z = 0.0f;
		//max.z = 1.0f;

		var bounds = new Bounds();
		bounds.SetMinMax( min, max );
		return bounds;
	}

}

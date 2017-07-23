using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author 
 * @author Paul Galatic
 * 
 * Class used to condense often-called methods into smaller chunks / lines such
 * to reduce cluttering of other files. However, all of these methods are 
 * effectively singletons, which is bad code practice. As core functionality is
 * implemented, and new best practices are learned, this file should be shrunk
 * down.
 * 
 * Some methods, like MouseIsOverUI(), are worthwhile in that they increase 
 * overall code readability. However, functions like IdentityToPrefab() should 
 * be phased out. TODO
 * **/
public class Utils : MonoBehaviour {

    private static Player playerOne;

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

    /// <summary>
    /// Draws a rectangle with the passed in color to the screen
    /// </summary>
    public static void DrawScreenRect(Rect rect, Color color) {
		GUI.color = color;
		GUI.DrawTexture(rect, WhiteTexture);
		GUI.color = Color.white;
	}

    /// <summary>
    /// Creates a border of a rectangle
    /// </summary>
    public static void DrawScreenRectBorder( Rect rect, float thickness, Color color )
	{
		// Top
		DrawScreenRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
		// Left
		DrawScreenRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
		// Right
		DrawScreenRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color);
		// Bottom
		DrawScreenRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
	}

    /// <summary>
    /// Returns a rectangle based on the 2 input screen positions
    /// 
    /// FIXME Currently unused
    /// </summary>
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

    /// <summary>
    /// Creates a bound, based on where the mouse dragged, that will contain 
    /// the object top be selected
    /// </summary>
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

    /// <summary>
    /// Returns whether or not the mouse is currently over a UI object.
    /// </summary>
    /// <returns>True if over UI, false otherwise.</returns>
    public static bool MouseIsOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    /// <summary>
    /// Returns a list of all units currently in the scene.
    /// </summary>
    public static List<Unit> AllUnits()
    {
        return new List<Unit>(FindObjectsOfType<Unit>());
    }

    /// <summary>
    /// Returns a list of all cities currently in the scene.
    /// </summary>
    public static List<City> AllCities()
    {
        return new List<City>(FindObjectsOfType<City>());
    }



    /// <summary>
    /// Returns the prefab associated with a particular type of Unit.
    /// </summary>
    public static MobileUnit IdentityToGameObject(string identity)
    {
        switch (identity)
        {
            case Twirl.IDENTITY:
                return Toolbox.TwirlPool.Rent();
            case Boomy.IDENTITY:
                throw new System.NotImplementedException();
        }

        throw new KeyNotFoundException("Bad value passed to UnitToPrefab()");
    }

}

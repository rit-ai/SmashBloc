using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Paul Galatic
 * 
 * This file contains the Invocation enum, which is designed specifically for 
 * use with Observers. When an event occurs, an Observable object sends an 
 * Invocation, along with some data, to all of its Observers, who then perform
 * behavior based on whether or not they are programmed to recognize the 
 * invocation.
 * 
 * Some invocations may be relevant to more than one Observer, but in general
 * they're only relevant to one.
 * 
 * All invocations are documented in the following form:
 *  *Entity: The GameObject(s) that sent the invocation.
 *  *(Observer): The misc data required by this Observer in order for the 
 *      invocation to be properly handled (otherwise, errors will be thrown).
 * **/
public enum Invocation {
    /********************************/
    // UI Observer && Game Observer //
    /********************************/

    /// <summary>
    /// Expresses that game is being paused.
    /// 
    /// Entity: CameraController
    /// (UIObserver -- Whether to open the pause menu or the end-game screen).
    /// </summary>
    PAUSE_AND_LOCK,

    /// <summary>
    /// Expresses that the user has toggled whether or not the game is paused.
    /// 
    /// Entity: CameraController
    /// </summary>
    TOGGLE_PAUSE,

    /// <summary>
    /// Expresses that the pause text should be displayed.
    /// 
    /// Entity: GameManager
    /// </summary>
    PAUSE_TEXT_ENABLE,

    /// <summary>
    /// Expresses that the pause text should not be displayed.
    /// 
    /// Entity: GameManager
    /// </summary>
    PAUSE_TEXT_DISABLE,

    /// <summary>
    /// Expresses that the user has entered a menu off of a main menu and
    /// a "back" button will not return them to the game
    /// 
    /// Entity: CameraController
    /// </summary>
    IN_SUBMENU,

    /// <summary>
    /// Expresses that the user has returned to the main menu, so if the user
    /// hits the "back" button, the user shall be able to return to the game
    /// 
    /// Entity: CameraController
    /// </summary>
    IN_MAINMENU,

    /*****************/
    // Game Observer //
    /*****************/

    /// <summary>
    /// Expresses that just one Unit has been selected.
    /// 
    /// Entity: Unit
    /// (GameObserver): Unit -- The Unit that was selected
    /// </summary>
    ONE_SELECTED,

    /// <summary>
    /// Expressses that units have been selected.
    /// 
    /// Entity: CameraController
    /// (GameObserver): List{Unit} -- The list of selected units
    /// </summary>
    UNITS_SELECTED,

    /// <summary>
    /// Expresses that all units have been deselected.
    /// 
    /// Entity: CameraController
    /// </summary>
    UNITS_DESELECTED,

    /// <summary>
    /// Expresses that a new destination has been set (presumably for units 
    /// that were previously selected).
    /// 
    /// Entity: RTS_Terrain
    /// </summary>
    DESTINATION_SET,

    /// <summary>
    /// Expresses that a city has been captured.
    /// 
    /// Entity: City
    /// (GameObserver): Team -- The team that captured the city
    /// </summary>
    CITY_CAPTURED,

    /// <summary>
    /// Expresses that the game is to be reset.
    /// 
    /// Entity: UIManager
    /// </summary>
    RESET_GAME,

    /// <summary>
    /// Expresses that an animation playing on the screen has finished, in case
    /// something in GameManager was waiting on it.
    /// 
    /// Entity: UIManager
    /// </summary>
    ANIMATION_FINISHED,

    /***************/
    // UI Observer //
    /***************/

    /// <summary>
    /// Expresses that the game is starting, so that UI Observer can play the 
    /// starting animation.
    /// 
    /// Entity: GameManager
    /// </summary>
    GAME_STARTING,

    /// <summary>
    /// Expresses that the game is ending, so that the UI Observer can play the 
    /// ending animation.
    /// 
    /// Entity: GameManager
    /// </summary>
    GAME_ENDING,

    /// <summary>
    /// Expresses that a menu should be opened to display information about a
    /// Unit.
    /// 
    /// Entity: Unit
    /// </summary>
    UNIT_MENU,

    /// <summary>
    /// Expresses that a menu should be opened to display information about a 
    /// City.
    /// 
    /// Entity: City
    /// </summary>
    CITY_MENU,

    /// <summary>
    /// Expresses that the target ring, which indicates the last set 
    /// destination, should be moved.
    /// 
    /// Entity: RTS_Terrain
    /// </summary>
    TARGET_RING,

    /// <summary>
    /// Expresses that all menus should be closed.
    /// 
    /// Entity: CameraController, RTS_Terrain.cs, GameManager.cs
    /// </summary>
    CLOSE_ALL
}

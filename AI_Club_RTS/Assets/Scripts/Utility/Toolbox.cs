using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * source: http://wiki.unity3d.com/index.php/Toolbox
 * @author Paul Galatic
 * 
 * This Toolbox houses single instances of classes to be referred to elsewhere
 * in code. Please think very, very carefully before modifying this file.
 * **/
public sealed class Toolbox : Singleton<Toolbox> {

    private Toolbox() { }

    private static UIManager uiManager;
    private static GameManager gameManager;
    private static ObjectPool pool;

    private static City cityPrefab;
    private static MobileUnit infantryPrefab;
    private static MobileUnit tankPrefab;

    // Public accessors and private variables to ensure that the contents of 
    // the variables will never change
    public static ObjectPool Pool
    {
        get { return pool; }
    }
    public static UIManager UIManager
    {
        get { return uiManager; }
    }
    public static GameManager GameManager
    {
        get { return gameManager; }
    }
    public static City CityPrefab
    {
        get { return cityPrefab; }
    }
    public static MobileUnit InfantryPrefab
    {
        get { return infantryPrefab; }
    }
    public static MobileUnit TankPrefab
    {
        get { return tankPrefab; }
    }

    /// <summary>
    /// TODO make UI Manager find or build all of its public variables.
    /// 
    /// Order in this function is EXTREMELY important.
    /// </summary>
    private void Awake()
    {
        cityPrefab = Resources.Load<City>("Prefabs/" + City.IDENTITY);
        infantryPrefab = Resources.Load<Infantry>("Prefabs/Units/" + Infantry.IDENTITY);
        tankPrefab = Resources.Load<Tank>("Prefabs/Units/" + Tank.IDENTITY);

        pool = gameObject.AddComponent<ObjectPool>();
        uiManager = FindObjectOfType<UIManager>();
        gameManager = gameObject.AddComponent<GameManager>();

        Debug.Assert(CityPrefab);
        Debug.Assert(InfantryPrefab);
        Debug.Assert(TankPrefab);
        Debug.Assert(gameManager);
        Debug.Assert(uiManager);
    }

}


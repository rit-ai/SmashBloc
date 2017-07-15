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
 * 
 * One and only one of this object should be placed into every Scene, as it 
 * constructs and initializes the Object Pools, Managers, and others.
 * **/
public sealed class Toolbox : Singleton<Toolbox> {

    private const int SMALL_POOL = 10;
    private const int MEDIUM_POOL = 50;

    private static ObjectPool<Infantry> infantryPool;
    private static ObjectPool<City> cityPool;
    private static UIManager uiManager;
    private static GameManager gameManager;
    private static UIObserver uiObserver;
    private static GameObserver gameObserver;

    private static City cityPrefab;
    private static MobileUnit infantryPrefab;
    private static MobileUnit tankPrefab;

    // Public accessors and private variables to ensure that the contents of 
    // the variables will never change
    public static ObjectPool<Infantry> InfantryPool
    {
        get { return infantryPool; }
    }
    public static ObjectPool<City> CityPool
    {
        get { return cityPool; }
    }
    public static UIManager UIManager
    {
        get { return uiManager; }
    }
    public static GameManager GameManager
    {
        get { return gameManager; }
    }
    public static UIObserver UIObserver
    {
        get { return uiObserver; }
    }
    public static GameObserver GameObserver
    {
        get { return gameObserver; }
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

    // A private constructor makes sure that this object will never be 
    // accidentally created in code
    private Toolbox() { }

    /// <summary>
    /// TODO make UI Manager find or build all of its public variables.
    /// 
    /// Order in this function is EXTREMELY important.
    /// </summary>
    private void Awake()
    {
        cityPrefab = Resources.Load<City>("Prefabs/Units/" + City.IDENTITY);
        infantryPrefab = Resources.Load<Infantry>("Prefabs/Units/" + Infantry.IDENTITY);
        tankPrefab = Resources.Load<Tank>("Prefabs/Units/" + Tank.IDENTITY);

        uiManager = FindObjectOfType<UIManager>();
        gameManager = gameObject.AddComponent<GameManager>();

        uiObserver = gameObject.AddComponent<UIObserver>();
        gameObserver = gameObject.AddComponent<GameObserver>();

        infantryPool = new ObjectPool<Infantry>(MakeInfantry, MEDIUM_POOL);
        cityPool = new ObjectPool<City>(MakeCity, SMALL_POOL);

        Debug.Assert(CityPrefab);
        Debug.Assert(InfantryPrefab);
        Debug.Assert(TankPrefab);
        Debug.Assert(gameManager);
        Debug.Assert(uiManager);
        Debug.Assert(uiObserver);
        Debug.Assert(gameObserver);
    }

    /// <summary>
    /// Constructs and returns an inactive Infantry game object.
    /// </summary>
    private Infantry MakeInfantry()
    {
        Infantry newInfantry = Instantiate(InfantryPrefab as Infantry);
        newInfantry.gameObject.SetActive(false);
        newInfantry.gameObject.AddComponent<InfantryPhysics>();
        newInfantry.AI = newInfantry.gameObject.AddComponent<UnitAI_Template>();
        newInfantry.AI.Body = newInfantry;
        newInfantry.Init();

        return newInfantry;
    }

    /// <summary>
    /// Constructs and returns an instantiated and disabled City.
    /// </summary>
    private City MakeCity()
    {
        City city = Instantiate(CityPrefab);
        city.gameObject.SetActive(false);
        city.Init();

        return city;
    }

}


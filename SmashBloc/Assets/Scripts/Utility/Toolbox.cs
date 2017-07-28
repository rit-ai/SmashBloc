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

    [Tooltip("Causes the game to reset automatically when it ends.")]
    public bool playContinuous;

    private const int SMALL_POOL = 10;
    private const int MEDIUM_POOL = 100;

    private static ObjectPool<Twirl> twirlPool;
    private static ObjectPool<City> cityPool;
    private static UIManager uiManager;
    private static GameManager gameManager;
    private static UIObserver uiObserver;
    private static GameObserver gameObserver;

    private static City cityPrefab;
    private static MobileUnit twirlPrefab;
    private static MobileUnit tankPrefab;

    private static RTS_Terrain terrain;

    GameObject cityPoolWrapper;
    GameObject twirlPoolWrapper;

    // Public accessors and private variables to ensure that the contents of 
    // the variables will never change
    public static ObjectPool<Twirl> TwirlPool
    {
        get { return twirlPool; }
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
    public static MobileUnit TwirlPrefab
    {
        get { return twirlPrefab; }
    }
    public static MobileUnit TankPrefab
    {
        get { return tankPrefab; }
    }
    public static RTS_Terrain Terrain
    {
        get { return terrain; }
    }

    // A private constructor makes sure that this object will never be 
    // accidentally created in code
    private Toolbox() { }

    /// <summary>
    /// TODO? make UI Manager find or build all of its public variables.
    /// 
    /// Order in this function is EXTREMELY important.
    /// </summary>
    private void Awake()
    {
        // First there was the land...
        terrain = GameObject.FindGameObjectWithTag(RTS_Terrain.TERRAIN_TAG).GetComponent<RTS_Terrain>();

        // ...and from the land came the prefabs...
        cityPrefab = Resources.Load<City>("Prefabs/Units/" + City.IDENTITY);
        twirlPrefab = Resources.Load<Twirl>("Prefabs/Units/" + Twirl.IDENTITY);
        tankPrefab = Resources.Load<Boomy>("Prefabs/Units/" + Boomy.IDENTITY);

        // ...and from the prefabs came those that managed them...
        uiManager = FindObjectOfType<UIManager>();
        gameManager = gameObject.AddComponent<GameManager>();

        // ...and from the managers came those that observed them...
        uiObserver = gameObject.AddComponent<UIObserver>();
        gameObserver = gameObject.AddComponent<GameObserver>();

        // ...and the observers grouped the prefabs into wrappers...
        twirlPoolWrapper = new GameObject("Twirl Pool");
        cityPoolWrapper = new GameObject("City Pool");

        // ...and the observers said that the prefabs would always be plenty...
        twirlPool = new ObjectPool<Twirl>(MakeTwirl, MEDIUM_POOL);
        cityPool = new ObjectPool<City>(MakeCity, SMALL_POOL);

        // ...and all of that is me.
        Debug.Assert(CityPrefab);
        Debug.Assert(TwirlPrefab);
        Debug.Assert(TankPrefab);
        Debug.Assert(gameManager);
        Debug.Assert(uiManager);
        Debug.Assert(uiObserver);
        Debug.Assert(gameObserver);
        
        // Toolbox.
    }

    private void Start()
    {
        gameManager.playContinuous = playContinuous;
    }

    /// <summary>
    /// Constructs and returns an inactive Twirl game object.
    /// </summary>
    private Twirl MakeTwirl()
    {
        Twirl newTwirl = Instantiate(TwirlPrefab as Twirl, twirlPoolWrapper.transform);
        newTwirl.gameObject.SetActive(false);
        newTwirl.gameObject.AddComponent<TwirlPhysics>();
        newTwirl.AI = newTwirl.gameObject.AddComponent<MobileAI_Basic>();
        newTwirl.AI.Body = newTwirl;
        newTwirl.Init();

        return newTwirl;
    }

    /// <summary>
    /// Constructs and returns an instantiated and disabled City.
    /// </summary>
    private City MakeCity()
    {
        City city = Instantiate(CityPrefab, cityPoolWrapper.transform);
        city.gameObject.SetActive(false);
        city.Init();

        return city;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    // Public 
    public Artillery ARTILLERY;
    public Bazooka BAZOOKA;
    public Infantry INFANTRY;
    public Recon RECON;
    public SupplyTruck SUPPLY_TRUCK;
    public Tank TANK;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Spawns a unit at the position of the object hosting this script.
    /// </summary>
    public void Spawn(Unit unit)
    {
        // Instantiate(unit, transform);
        if (unit is Infantry)
        {
            Instantiate(INFANTRY, transform.position, transform.rotation);
        } else if (unit is Tank)
        {
            Instantiate(TANK, transform.position, transform.rotation);
        }
    }


}

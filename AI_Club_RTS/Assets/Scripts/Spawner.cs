using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject m_Cube;
    public GameObject m_Pill;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Spawns a unit at the position of the object hosting this script.
    /// </summary>
    public void Spawn<Unit>(Unit unit)
    {
        // Instantiate(unit, transform);
        if (unit is Infantry)
        {
            Instantiate(m_Cube, transform);
        } else if (unit is Tank)
        {
            Instantiate(m_Pill, transform);
        }
    }


}

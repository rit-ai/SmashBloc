using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold_Manager : MonoBehaviour {

	public int currentGold;
	const int second = 1;
	float counter;

	void Start () {
		counter = 0;
		currentGold = 0;
	}
	
	void Update () {
		UpdateGold ();
	}

	// Increments a players gold each second
	void UpdateGold() {
		counter += Time.deltaTime;
		if (counter >= second) {
			currentGold++;
			counter = 0;
		}
	}
}

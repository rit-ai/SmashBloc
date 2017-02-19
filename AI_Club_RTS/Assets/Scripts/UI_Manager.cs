using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

	GameObject sceneManager;
	Gold_Manager gold_Manager;

	Text gold;


	void Start () {
		sceneManager = GameObject.Find ("Scene_Manager");
		gold = GameObject.Find("Gold_Text").GetComponent<Text> ();
		gold_Manager = sceneManager.GetComponent<Gold_Manager> ();
		Debug.Log (sceneManager.name);
		Debug.Log (gold_Manager.currentGold);
	}

	void Update () {
		gold.text = "" +gold_Manager.currentGold;
	}
}

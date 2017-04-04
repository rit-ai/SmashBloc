using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

	GameObject sceneManager;
	Gold_Manager gold_Manager;
	Text gold;
	public Camera camera;
	public GameObject spawnable;
	Button button;

	void Start () {
		sceneManager = GameObject.Find ("Scene_Manager");
		gold = GameObject.Find("Gold_Text").GetComponent<Text> ();
		gold_Manager = sceneManager.GetComponent<Gold_Manager> ();
		Debug.Log (sceneManager.name);
		Debug.Log (gold_Manager.currentGold);
		this.button = this.transform.Find("Button").GetComponent<Button>();
		button.onClick.AddListener(SpawnUnit);
	}

	void Update () {
		gold.text = "" +gold_Manager.currentGold;

		if(Input.GetMouseButtonUp(0)) {
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit)) {
				if(hit.transform.tag == "Unit") {
					button.gameObject.SetActive(true);
					Debug.Log("buttons found: ");
				} else {
					button.gameObject.SetActive(false);
				}
			}
		}
	}

	// Spawns a new unit if gold_Manager has enough gold
	private void SpawnUnit() {
		if(gold_Manager.SpendGold(5)) {
			Debug.Log("spawn");
			GameObject newUnit = Instantiate(spawnable, new Vector3(-9.5f, 1, 10), Quaternion.identity);
		}
	}


}

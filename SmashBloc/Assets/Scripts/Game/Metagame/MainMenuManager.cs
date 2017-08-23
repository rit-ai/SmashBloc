using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * @author Tory Leo
 * 
 * Class designed to handle navigation and overall management of the Main Menu Scene
 * **/

public class MainMenuManager : MonoBehaviour
{

    // **         //
    // * FIELDS * //
    //         ** //

    public Button startButton;
    public Button quitButton;

    // Initialize only once
    private void Awake()
    {
        startButton.onClick.AddListener(StartButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Loads the demo scene (starts the game)
    /// </summary>
    private void StartButtonPressed()
    {
        SceneManager.LoadScene("DemoScene");
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    private void QuitButtonPressed()
    {
        Application.Quit();
    }
}

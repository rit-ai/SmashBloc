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
    public Button optionsButton;
    public Button quitButton;

    private bool inOptions;


    // **          //
    // * METHODS * //
    //          ** //

    // Initialize only once
    private void Awake()
    {
        inOptions = false;
        startButton.onClick.AddListener(StartButtonPressed);
        optionsButton.onClick.AddListener(OptionsButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    private void Update()
    {
        if (inOptions && Input.GetKeyDown(KeyCode.Escape))
        {
            startButton.gameObject.SetActive(true);
            optionsButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            inOptions = false;
        }
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

    /// <summary>
    /// Nullifies the other buttons/activates them
    /// TODO: Create a more modular menu/options system that is re-usable
    /// </summary>
    private void OptionsButtonPressed()
    {
        startButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        inOptions = true;
    }
}

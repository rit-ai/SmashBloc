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
    public Toggle enableDevToggle;
    public GameObject MainPanel;
    public GameObject OptionsPanel;



    // **          //
    // * METHODS * //
    //          ** //

    // Initialize only once
    private void Awake()
    {
        MainPanel.gameObject.SetActive(true);
        OptionsPanel.gameObject.SetActive(false);
        if (PlayerPrefs.HasKey("enableDevToggle"))
            enableDevToggle.isOn = (PlayerPrefs.GetInt("enableDevToggle") == 1);

        startButton.onClick.AddListener(StartButtonPressed);
        optionsButton.onClick.AddListener(OptionsButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscKeyPressed();
        }
    }

    void OnDisable()
    {
        if(enableDevToggle.isOn)
            PlayerPrefs.SetInt("enableDevToggle", 1);
        else
            PlayerPrefs.SetInt("enableDevToggle", 0);
    }

    /// <summary>
    /// Called from update whenever KeyCode.Escape is pressed. Will go back to
    /// previous panel if not in MainMenuPanel
    /// </summary>
    private void EscKeyPressed()
    {
        if(OptionsPanel.gameObject.activeSelf)
        {
            OptionsPanel.gameObject.SetActive(false);
            MainPanel.gameObject.SetActive(true);
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
        MainPanel.gameObject.SetActive(false);
        OptionsPanel.gameObject.SetActive(true);
    }
}

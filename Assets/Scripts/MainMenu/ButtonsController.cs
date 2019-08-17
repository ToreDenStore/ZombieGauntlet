using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject gameInfoScreen;
    public GameObject introScreen;

    public void StartGame()
    {
        print("Go to info screen");
        startScreen.SetActive(false);
        gameInfoScreen.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameInfoScreen.activeSelf == true)
        {
            print("Go to intro screen");
            GoToIntroScreen();
        } else if (Input.GetMouseButtonDown(0) && introScreen.activeSelf == true)
        {
            print("Start game");
            StartLevel();
        }
    }

    private void GoToIntroScreen()
    {
        gameInfoScreen.SetActive(false);
        introScreen.SetActive(true);
    }

    public void StartLevel()
    {
        print("Loading level 1...");
        SceneManager.LoadScene("Level1");
        print("Loaded Level 1!");
    }

    public void QuitGame()
    {
        print("Exiting game...");
        Application.Quit();
        print("Exited!");
    }
}

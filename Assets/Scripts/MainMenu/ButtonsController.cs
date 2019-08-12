using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsController : MonoBehaviour
{
    public void StartGame()
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

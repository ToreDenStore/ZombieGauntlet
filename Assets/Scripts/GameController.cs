using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    /*
     * https://answers.unity.com/questions/811686/how-do-i-toggle-my-pause-menu-with-escape.html 
    */
    
    public GameObject pausedCanvas;
    public GameObject winCanvas;
    public GameObject viewMask;
    public int secondsBeforeQuitGame;

    private bool gameLost;
    private bool paused;

    //Music
    private AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        gameLost = false;
        viewMask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            //print("Escape pressed");
            if (!paused)
            {
                PauseGameButtonClick();
            } else
            {
                ResumeGameButtonClick();
            }
        }

    }

    public bool IsPaused()
    {
        return this.paused;
    }

    public void PauseGameButtonClick()
    {
        pausedCanvas.SetActive(true);
        PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        paused = true;
    }

    public void ResumeGameButtonClick()
    {
        pausedCanvas.SetActive(false);
        UnPauseGame();
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1f;
        paused = false;
    }

    public void QuitGame()
    {
        print("Quitting level...");
        SceneManager.LoadScene("MainMenu");
    }

    public void LoseGame()
    {
        if (!gameLost)
        {
            gameLost = true;
            print("You lost!");
            StartCoroutine(WaitForSecondsBeforeQuitGame());
        }
    }

    public void WinGame()
    {
        //Display win game text
        print("You made it to the train!");
        winCanvas.SetActive(true);
        StartCoroutine(WaitForSecondsBeforeQuitGame());
        PauseGame();
    }

    private IEnumerator WaitForSecondsBeforeQuitGame()
    {
        print("Loading new scene in " + secondsBeforeQuitGame + " seconds");
        yield return new WaitForSecondsRealtime(secondsBeforeQuitGame);
        SceneManager.LoadScene("MainMenu");
    }

}

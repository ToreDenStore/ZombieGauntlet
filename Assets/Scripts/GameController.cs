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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            //print("Escape pressed");
            if (!paused)
            {
                PauseGame();
            } else
            {
                ResumeGame();
            }
        }

    }

    public bool isPaused()
    {
        return this.paused;
    }

    public void PauseGame()
    {
        pausedCanvas.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void ResumeGame()
    {
        pausedCanvas.SetActive(false);
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

    private IEnumerator WaitForSecondsBeforeQuitGame()
    {
        yield return new WaitForSeconds(secondsBeforeQuitGame);
        SceneManager.LoadScene("MainMenu");
    }

}

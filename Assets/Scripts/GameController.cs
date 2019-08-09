using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Music
    private AudioSource backgroundMusic;

    public int secondsBeforeQuitGame;

    private bool gameLost;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        gameLost = false;
    }

    // Update is called once per frame
    void Update()
    {

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
        SceneManager.LoadScene("Level1");
    }

}

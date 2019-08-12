using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedController : MonoBehaviour
{
    /*
     * https://answers.unity.com/questions/811686/how-do-i-toggle-my-pause-menu-with-escape.html 
    */

    private bool paused;

    public GameObject pausedCanvas;

    // Start is called before the first frame update
    void Start()
    {
        paused = pausedCanvas.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            print("Escape pressed");
            if (!paused)
            {
                pausedCanvas.SetActive(true);
                Time.timeScale = 0f;
                paused = true;
            }
        }
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


}

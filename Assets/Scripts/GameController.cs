using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //Music
    private AudioSource backgroundMusic;

    //Time
    private string currentTimeDisplayed = "<time>";
    private Text timerText;

    public GameObject timerTextObject;
    public int trainLeavesInMinutes;

    // Start is called before the first frame update
    void Start()
    {
        timerText = timerTextObject.GetComponent<Text>();
        backgroundMusic = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        DateTime nowTime = new DateTime().AddMinutes(trainLeavesInMinutes).AddSeconds(Mathf.RoundToInt(Time.time) * -1);
        UpdateScreenTextWithNewGameTime(nowTime);
    }

    void UpdateScreenTextWithNewGameTime(DateTime gameTime)
    {
        string displayTime = getDisplayTime(gameTime);
        if (!displayTime.Equals(currentTimeDisplayed))
        {
            timerText.text = timerText.text.Replace(currentTimeDisplayed, displayTime);
            currentTimeDisplayed = displayTime;
        }
    }

    string getDisplayTime(DateTime time)
    {
        return time.Minute.ToString() + ":" + time.Second.ToString().PadLeft(2, '0');
    }
}

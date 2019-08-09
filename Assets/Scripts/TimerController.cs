using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    //Time
    private string currentTimeDisplayed = "<time>";
    private Text timerText;
    
    public int trainLeavesInMinutes;
    public GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= trainLeavesInMinutes * 60)
        {
            print("You missed the train!");
            gameController.loseGame();
        }
        DateTime nowTime = new DateTime().AddMinutes(trainLeavesInMinutes).AddSeconds(Mathf.RoundToInt(Time.time) * -1);
        //nowTime doesn't go down below 1 second, could be done better
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

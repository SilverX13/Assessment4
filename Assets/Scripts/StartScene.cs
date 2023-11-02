using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    public TextMeshProUGUI BestTimeText;

    void Start()
    {
        UpdateHighScoreUI();
        UpdateBestTimeUI();
    }

    void UpdateHighScoreUI()
    {
        int highScore = GetHighScore();
        HighScoreText.text = "High Score: " + highScore;
    }

    void UpdateBestTimeUI()
    {
        float bestTime = GetBestTime();
        BestTimeText.text = "Best Time: " + FormatTime(bestTime);
    }

    int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    float GetBestTime()
    {
        return PlayerPrefs.GetFloat("BestTime", float.MaxValue);
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 100) % 100);

        return minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
    }
}

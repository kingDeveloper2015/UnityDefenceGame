using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

    private TimerData timerData;
    private float levelTime;
    private float startTime;
    private bool running;
    public Text txtTimer;
    public Text txtFastestTime;

    public void Start() {
        PlayerController.GameStopEvent += GameStop;
        timerData = new TimerData(SceneManager.GetActiveScene().buildIndex);
        TimeSpan t = TimeSpan.FromSeconds(timerData.GetFastestTime());
        string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Minutes, t.Seconds, t.Milliseconds.ToString("000").Substring(0, 2));
        try {
            txtFastestTime.text = timeFormatted.ToString();
        } catch (Exception ex) {
            Debug.Log("Exception:" + ex.Message);
        }
        if (timerData.GetFastestTime() == 0) {
            txtFastestTime.gameObject.SetActive(false);
        } else {
            txtFastestTime.gameObject.SetActive(true);
        }
        startTime = Time.fixedTime;
        running = true;
    }

    public void FixedUpdate() {
        if (running) {
            levelTime = Time.fixedTime - startTime;
        }
    }

    public void Update() {
        if (running) {
            TimeSpan t = TimeSpan.FromSeconds(levelTime);
            string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Minutes, t.Seconds, t.Milliseconds.ToString("000").Substring(0, 2));
            txtTimer.text = timeFormatted.ToString();
        }


    }

    private void GameStop(bool win) {
        running = false;
        try {
            if (win && (levelTime < timerData.GetFastestTime() || timerData.GetFastestTime() == 0)) {
                timerData.SetFastestTime(levelTime);
                TimeSpan t = TimeSpan.FromSeconds(timerData.GetFastestTime());
                string timeFormatted = string.Format("{0:D2} :{1:D2}:{2:D2}", t.Minutes, t.Seconds, t.Milliseconds.ToString("000").Substring(0, 2));
                txtFastestTime.text = timeFormatted;
				txtTimer.text = AnalyticsManager.instance.CompletedLevelTime;
                txtFastestTime.gameObject.SetActive(true);
				Debug.LogError("The timer is " + timeFormatted);
            }
        } catch (Exception ex) {
            Debug.Log("VerifyScoreException:" + ex.Message);
        }

    }
}

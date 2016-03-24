using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class InGameLevelTextItem : MonoBehaviour {

    public Text shadowText;
    public Text levelText;
	public Text timeText;

	public static InGameLevelTextItem instance;
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
    public void SetTextValues(string _value)
    {
        shadowText.text = _value;
        levelText.text = _value;
    }

	public void SetTimeValues()
	{
		TimerData timerData = new TimerData(SceneManager.GetActiveScene().buildIndex);
		TimeSpan t = TimeSpan.FromSeconds(timerData.GetFastestTime());
		string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Minutes, t.Seconds, t.Milliseconds.ToString("000").Substring(0, 2));
		try {
			timeText.text = "Best:" + timeFormatted.ToString();
		} catch (Exception ex) {
			Debug.Log("Exception:" + ex.Message);
		}
		if (timerData.GetFastestTime() == 0) {
			timeText.gameObject.SetActive(false);
		} else {
			timeText.gameObject.SetActive(true);
		}
	}

	public void SetBestTimeValue()
	{
		TimerData timerData = new TimerData(SceneManager.GetActiveScene().buildIndex);
		TimeSpan t = TimeSpan.FromSeconds(timerData.GetFastestTime());
		string timeFormatted = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Minutes, t.Seconds, t.Milliseconds.ToString("000").Substring(0, 2));
		try {
			timeText.text = "Best:" + timeFormatted.ToString();
		} catch (Exception ex) {
			Debug.Log("Exception:" + ex.Message);
		}
		if (timerData.GetFastestTime() == 0) {
			timeText.text = "Best: ##:##:##";
			timeText.gameObject.SetActive(true);
		} else {
			timeText.gameObject.SetActive(true);
		}
	}
}

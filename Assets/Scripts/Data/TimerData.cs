using UnityEngine;

public class TimerData {

    private int levelID;
    private float fastestTime;

    public TimerData(int level) {
        this.levelID = level;
        Load(this.levelID);
    }

    public TimerData(int level, float miliseconds) {
        this.levelID = level;
        this.fastestTime = miliseconds;
        Save();
    }

    public void Load(int level) {
        this.levelID = level;
        this.fastestTime = PlayerPrefs.GetFloat("level" + this.levelID.ToString(), 0);
    }

    public void Save() {
        PlayerPrefs.SetFloat("level" + this.levelID.ToString(), this.fastestTime);
        PlayerPrefs.Save();
    }

    public float GetFastestTime() {
        return this.fastestTime;
    }

    public void SetFastestTime(float miliseconds) {
        this.fastestTime = miliseconds;
        Save();
    }
}

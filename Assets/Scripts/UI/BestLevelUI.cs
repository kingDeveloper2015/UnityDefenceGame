using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BestLevelUI : MonoBehaviour {

    Text scoreText;

    void Awake()
    {
        scoreText = GetComponent<Text>();
    }
	// Use this for initialization
	void Start () {
        int levelScore = PlayerPrefs.GetInt("BestLevelScore", 0);
        scoreText.text = "LEVEL " + levelScore.ToString("000");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

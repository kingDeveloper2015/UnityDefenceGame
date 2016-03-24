using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController Instance;

    public bool gameOver;

    public int currentLevel = 0;
    public GameObject LevelTextPrefab;
    public GameObject PointIndicatorPrefab;

    private Transform CanvasTransform;

    public void Awake() {
      //  PlayerPrefs.DeleteAll();
        Instance = this;
        string LevelName = SceneManager.GetActiveScene().name;
        LevelName = LevelName.Replace("Level", "");
        int result = 0;
        int.TryParse(LevelName, out result);
        currentLevel = result;

        CanvasTransform = FindObjectOfType<Canvas>().transform;
        Instantiate(PointIndicatorPrefab);
    }

    public void Start() {
        GameObject temp = Instantiate(LevelTextPrefab);
        temp.transform.SetParent(CanvasTransform, false);
		string totalLevel = Resources.Load<TextAsset> ("TotalLevel").text;
		temp.GetComponent<InGameLevelTextItem>().SetTextValues("Level " + currentLevel.ToString() + " / " + totalLevel);// " / 40");
		//temp.GetComponent<InGameLevelTextItem>().SetTimeValues();
        if (AdsController.instance != null) { AdsController.instance.ShowTopBanner(); }
    }


}

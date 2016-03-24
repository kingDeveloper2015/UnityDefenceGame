using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    GameObject gameOver, gameWin;
    [SerializeField]
    float MovementSpeed = 10f, delayMovement = 0.2f, time1, time2;
    public ReviewController reviewController;
    private Transform _mainCameraTransform;

    public List<Vector3> points;
    private Vector3 previouspos;
    private Animator _anim;
    private float levelStartingTime;
    Vector2 stopOnReleaseVector;
    Rigidbody2D rigidBody2D;
    public float directionThreshold;

    [SerializeField]
    int animCallNumber;

    [SerializeField]
    bool movementCalled, movePlayer;

    public delegate void IndicatorAction(Vector3 pos);
    public static event IndicatorAction OnPointSelected;
    private List<Vector3> LastestPositions = new List<Vector3>();


    public delegate void DelegateGameStop(bool stopped);
    public static event DelegateGameStop GameStopEvent;

	void Start() {
//		PlayerPrefs.DeleteAll ();
        previouspos = transform.position;
        SendLevelStartedToAnalytics();
    }

    void OnEnable() {
        _anim = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetMouseButtonUp(0) && !GameController.Instance.gameOver) {
            movementCalled = true;
            movePlayer = false;
            Vector3 pos = Input.mousePosition;
            pos.z = 10;
            pos = Camera.main.ScreenToWorldPoint(pos);
            ShowIndicator(pos);
            points.Clear();
            points.Add(pos);
        }
    }
    void FixedUpdate() {

        if (!GameController.Instance.gameOver) {
            if (movePlayer) {

                if (!movementCalled)
                    transform.position = Vector3.MoveTowards(transform.position, points[0], MovementSpeed * Time.fixedDeltaTime);

                if (transform.position == points[0]) {
                    setPlayerToIdle();
                } else {
                    if (IsDeathEnd(transform.position)) {
                        setPlayerToIdle();
                    }
                }
            }

            if (points.Count > 0 && movementCalled) {
                if ((points[0].x - transform.position.x > directionThreshold) && (points[0].y - transform.position.y > directionThreshold)) {
                    _anim.SetTrigger("RunTopRight");
                    animCallNumber = 1;
                } else if ((points[0].x - transform.position.x > directionThreshold) && (points[0].y - transform.position.y < -directionThreshold)) {
                    _anim.SetTrigger("RunDownRight");
                    animCallNumber = 2;
                } else if ((points[0].x - transform.position.x < -directionThreshold) && (points[0].y - transform.position.y > directionThreshold)) {
                    _anim.SetTrigger("RunTopLeft");
                    animCallNumber = 3;
                } else if ((points[0].x - transform.position.x < -directionThreshold) && (points[0].y - transform.position.y < -directionThreshold)) {
                    _anim.SetTrigger("RunDownLeft");
                    animCallNumber = 4;
                } else
                      if (((points[0].x - transform.position.x >= -directionThreshold) && (points[0].x - transform.position.x <= directionThreshold)) && (points[0].y - transform.position.y > directionThreshold)) {
                    _anim.SetTrigger("RunTop");
                    animCallNumber = 5;
                } else if (((points[0].x - transform.position.x >= -directionThreshold) && (points[0].x - transform.position.x <= directionThreshold)) && (points[0].y - transform.position.y < directionThreshold)) {
                    _anim.SetTrigger("RunDown");
                    animCallNumber = 6;
                } else if ((points[0].x - transform.position.x > directionThreshold) && ((points[0].y - transform.position.y < directionThreshold) || (points[0].y - transform.position.y > -directionThreshold))) {
                    _anim.SetTrigger("RunRight");
                    animCallNumber = 7;
                } else if ((points[0].x - transform.position.x < 0) && ((points[0].y - transform.position.y < directionThreshold) || (points[0].y - transform.position.y > -directionThreshold))) {
                    _anim.SetTrigger("RunLeft");
                    animCallNumber = 8;
                }

                movementCalled = false;
                movePlayer = true;
            }

        }
    }

    private bool IsPointsAlmostSame(Vector3 pointOne, Vector3 pointTwo) {

        float valueOne = pointOne.x - pointTwo.x;
        float valueTwo = pointOne.y - pointTwo.y;

        if (Mathf.Abs(valueOne) < 0.01f && Mathf.Abs(valueTwo) < 0.01f) {
            return true;
        } else {
            return false;
        }
    }

    private bool IsDeathEnd(Vector3 pos) {
        LastestPositions.Add(pos);
        if (LastestPositions.Count > 2) {
            int i = LastestPositions.Count - 1;
            if (IsPointsAlmostSame(LastestPositions[i], LastestPositions[i - 1]) && IsPointsAlmostSame(LastestPositions[i], LastestPositions[i - 2])) {
                LastestPositions.Clear();
                return true;
            }
        }
        return false;
    }
    void setPlayerToIdle() {
        IdleAnimEffects();
        movePlayer = false;
        movementCalled = true;
        points.Clear();
        LastestPositions.Clear();

    }
    void IdleAnimEffects() {
        switch (animCallNumber) {
            case 1:
            _anim.SetTrigger("IdleTopRight");
            break;
            case 2:
            _anim.SetTrigger("IdleDownRight");
            break;
            case 3:
            _anim.SetTrigger("IdleTopLeft");
            break;
            case 4:
            _anim.SetTrigger("IdleDownLeft");
            break;
            case 5:
            _anim.SetTrigger("IdleTop");
            break;
            case 6:
            _anim.SetTrigger("IdleDown");
            break;
            case 7:
            _anim.SetTrigger("IdleRight");
            break;
            case 8:
            _anim.SetTrigger("IdleLeft");
            break;
        }
    }

    public void OnTriggerEnter2D(Collider2D coll) {
        switch (coll.tag) {
            case "Enemy":
            IdleAnimEffects();
            EnemyEffects();
            break;
            case "FinishPoint":
            IdleAnimEffects();
            FinishPointsEffects();
            break;
        }
    }

    void EnemyEffects() {
        GameStopEvent(false); //delegate
        GameController.Instance.gameOver = true;


        gameOver.SetActive(true);
        gameOver.transform.FindChild("Background").GetComponent<RectTransform>().DOLocalMove(Vector3.zero, 0.25f);
        gameOver.transform.FindChild("Home").GetComponent<RectTransform>().DOLocalMoveX(0, 0.25f);
        SendLevelFailedToAnalytics();
    }

    void FinishPointsEffects() {
        GameStopEvent(true); //delegate
        GameController.Instance.gameOver = true;
        gameWin.SetActive(true); // Just For Time Being
        gameWin.transform.FindChild("Background").GetComponent<RectTransform>().DOLocalMove(Vector3.zero, 0.25f);
        gameWin.transform.FindChild("Home").GetComponent<RectTransform>().DOLocalMoveX(0, 0.25f);
        gameWin.transform.FindChild("NextLevel").GetComponent<RectTransform>().DOLocalMoveX(0, 0.25f);
        if (PlayerPrefs.GetInt("BestLevelScore", 0) < GameController.Instance.currentLevel) {
            PlayerPrefs.SetInt("BestLevelScore", GameController.Instance.currentLevel);
        }

        SendLevelClearedToAnalytics();
        if (reviewController != null) {
            bool reviewAsked = PlayerPrefs.GetInt("reviewAsked", 0) == 1;
            Debug.Log("reviewAsked: " + reviewAsked);
            if (!reviewAsked) {
                Debug.Log("reviewAsked: " + reviewAsked);
                reviewController.ShowReview();
                PlayerPrefs.SetInt("reviewAsked", 1);
                PlayerPrefs.Save();
            }
        }
    }

    void SendLevelStartedToAnalytics() {
        AnalyticsItem item = new AnalyticsItem();
        item.Desc = "Started Level";
        item.Value = SceneManager.GetActiveScene().name;
        levelStartingTime = Time.realtimeSinceStartup;

        if (AnalyticsManager.instance != null) {
            AnalyticsManager.instance.SendItemToAnaytics(item);
            AnalyticsManager.instance.StoreLastLevel(item.Value);
        }

    }

    void SendLevelClearedToAnalytics() {
        AnalyticsItem item = new AnalyticsItem();
        item.Desc = "Cleared Level : " + SceneManager.GetActiveScene().name;
        item.Value = "Level Time : " + (Time.realtimeSinceStartup - levelStartingTime).ToString();
		string time = (Time.realtimeSinceStartup - levelStartingTime).ToString();
		PlayerPrefs.SetString (SceneManager.GetActiveScene().name, time );

		if (AnalyticsManager.instance != null)
            AnalyticsManager.instance.SendItemToAnaytics(item);
    }
    void SendLevelFailedToAnalytics() {
        AnalyticsItem item = new AnalyticsItem();
        item.Desc = "Failed Level : " + SceneManager.GetActiveScene().name;
        item.Value = "Level Time : " + (Time.realtimeSinceStartup - levelStartingTime).ToString();
        item.ExtraValue = "Player Position : " + transform.position;
        if (AnalyticsManager.instance != null)
            AnalyticsManager.instance.SendItemToAnaytics(item);
    }

    void ShowIndicator(Vector3 pos) {
        if (OnPointSelected != null)
            OnPointSelected(pos);
    }
}

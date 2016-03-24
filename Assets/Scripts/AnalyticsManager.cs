using UnityEngine;
using System.Collections;
using Analytics;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class AnalyticsItem
{
    public string Desc { get; set; }
    public string Value { get; set; }
    public string ExtraValue { get; set; }

    public AnalyticsItem()
    {
        Desc = "";
        Value = "";
        ExtraValue = "";
    }
}
public class AnalyticsManager : MonoBehaviour
{

    public static AnalyticsManager instance;

    [Header("Flurry Settings")]
    [SerializeField]
    private string _iosApiKey = string.Empty;
    [SerializeField]
    private string _androidApiKey = string.Empty;
	public string CompletedLevelTime { get; set;}

    public delegate void AnalyticsActions();
    public static event AnalyticsActions OnFreeSkip;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
            IAnalytics service = Flurry.Instance;

            AssertNotNull(service, "Unable to create Flurry instance!", this);
            Assert(!string.IsNullOrEmpty(_iosApiKey), "_iosApiKey is empty!", this);
            Assert(!string.IsNullOrEmpty(_androidApiKey), "_androidApiKey is empty!", this);

            service.SetLogLevel(LogLevel.All);
            service.StartSession(_iosApiKey, _androidApiKey);
#endif
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
        int GameStarted = PlayerPrefs.GetInt("GameStarted", 1);

#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
        IAnalytics service = Flurry.Instance;
        service.LogEvent("event", new Dictionary<string, string>
            {
            #if UNITY_5
            	{ "AppVersion", Application.version },
            #endif
                { "UnityVersion", Application.unityVersion },
                { "Game Start Count", GameStarted.ToString() }
            });
#endif
        if (GameStarted == 3)
        {
            if (OnFreeSkip != null)
                OnFreeSkip();
        }
        GameStarted++;
        PlayerPrefs.SetInt("GameStarted", GameStarted);
    }

    /// <summary>
    /// Send Events to anaytics side
    /// </summary>
    /// <param name="item"></param>
    public void SendItemToAnaytics(AnalyticsItem item)
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
        if (item != null)
        {
            IAnalytics service = Flurry.Instance;
            service.LogEvent("event", new Dictionary<string, string>
            {
            #if UNITY_5
            	{ "AppVersion", Application.version },
            #endif
                { "UnityVersion", Application.unityVersion },
                {"Description", item.Desc },
                {"Info", item.Value },
                { "Extra info", item.ExtraValue } 
                
            });
            
            UnityEngine.Debug.Log("item Desc " + item.Desc);
            UnityEngine.Debug.Log("item value " + item.Value);

			if(item.Desc.Contains("Cleared Level")){
				CompletedLevelTime = item.Value;
				if(InGameLevelTextItem.instance != null){
					UnityEngine.Debug.LogError("it is not null");
					InGameLevelTextItem.instance.SetTimeValues();
				}
			}else if (item.Desc.Contains("Failed Level")){
				CompletedLevelTime = "Best: "+ PlayerPrefs.GetString (SceneManager.GetActiveScene().name, "##:##:##" );
				if(InGameLevelTextItem.instance != null){
					UnityEngine.Debug.LogError("it is not null");
					InGameLevelTextItem.instance.SetBestTimeValue();
				}
			}

        }
#endif
    }

    public void BuyKeyItemCalled()
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
        IAnalytics service = Flurry.Instance;
        service.LogEvent("event", new Dictionary<string, string>
            {
            #if UNITY_5
            	{ "AppVersion", Application.version },
            #endif
                { "UnityVersion", Application.unityVersion },
                {"In App Purchase", "Buy Key Item Screen Called" }
            });
#endif
    }

    public void BuyKeyItemCanceled()
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
        IAnalytics service = Flurry.Instance;
        service.LogEvent("event", new Dictionary<string, string>
            {
            #if UNITY_5
            	{ "AppVersion", Application.version },
            #endif
                { "UnityVersion", Application.unityVersion },
                {"In App Purchase", "Buy Key Item Screen Closed" }
            });
#endif
    }
    
    public void KeyItemBought()
    {
        int KeyItemBoughtCount = PlayerPrefs.GetInt("KeyItemBoughtCount", 0);
        KeyItemBoughtCount++;
        PlayerPrefs.SetInt("KeyItemBoughtCount", KeyItemBoughtCount);
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
        IAnalytics service = Flurry.Instance;
        service.LogEvent("event", new Dictionary<string, string>
            {
            #if UNITY_5
            	{ "AppVersion", Application.version },
            #endif
                { "UnityVersion", Application.unityVersion },
                {"In App Purchase", "Buy Key Item Bought" },
                {"Count", KeyItemBoughtCount.ToString() },
                {"Played Last Level",   PlayerPrefs.GetString("PlayedLastLevel", "Level1") },
                {"Session Time", Time.realtimeSinceStartup.ToString() }
            });
#endif   
    }

    public void StoreLastLevel(string levelName)
    {
        PlayerPrefs.SetString("PlayedLastLevel", levelName);
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void OnDisable()
    {
        Application.logMessageReceived += LogMessageReceived;
    }

    public void OnEnable()
    {
        Application.logMessageReceived += LogMessageReceived;
    }

    private void LogMessageReceived(string condition, string stackTrace, LogType type)
    {
#if UNITY_IOS || UNITY_ANDROID || UNITY_EDITOR
        IAnalytics service = Flurry.Instance;
        string error = "{\"condition\" : \"" + condition + "\", \"stackTrace\": \"" + stackTrace + "\", \"type\": " + type + "\"}";
        service.LogError("Application Error", error, this);
#endif
    }

    /// <summary>
    /// service.LogUserID("Github User");
    // service.LogUserAge(24);
    // service.LogUserGender(UserGender.Male);
    //			service.LogEvent("event", new Dictionary<string, string>
    //			{
    //#if UNITY_5
    //			    { "AppVersion", Application.version },
    //#endif
    //                { "UnityVersion", Application.unityVersion }
    //			});
    //		}

    //			service.BeginLogEvent("timed-event");
    //			service.EndLogEvent("timed-event");
    //			service.LogError("test-script-error", "Test Error", this);
    /// </summary>



    #region [Assert Methods]
    [Conditional("UNITY_EDITOR")]
    private void Assert(bool condition, string message, Object context)
    {
        if (condition)
        {
            return;
        }

        UnityEngine.Debug.LogError(message, context);
    }

    [Conditional("UNITY_EDITOR")]
    private void AssertNotNull(object target, string message, Object context)
    {
        Assert(target != null, message, context);
    }
    #endregion
}

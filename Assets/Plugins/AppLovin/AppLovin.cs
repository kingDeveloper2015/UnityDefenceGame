/**
 * AppLovin Unity Plugin C# Wrapper
 *
 * @author David Anderson, Matt Szaro
 * @version 3.2.0
 */

using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AppLovin {
    public const float AD_POSITION_CENTER  = -10000;
    public const float AD_POSITION_LEFT  = -20000;
    public const float AD_POSITION_RIGHT  = -30000;
    public const float AD_POSITION_TOP    = -40000;
	public const float AD_POSITION_BOTTOM = -50000;

	// Do not modify.
	private const char _InternalPrimarySeparator   = (char) 28;
	
	// Do not modify.
	private const char _InternalSecondarySeparator = (char) 29;
	
	#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern void _AppLovinShowAd ();
		
		[DllImport ("__Internal")]
		private static extern void _AppLovinHideAd ();
		
		[DllImport ("__Internal")]
		private static extern void _AppLovinShowInterstitial ();
		
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetAdPosition(float x, float y);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetAdWidth(float width);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetSdkKey (string sdkKey);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinInitializeSdk ();
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetVerboseLoggingOn (string isVerboseLogging);
	
		[DllImport ("__Internal")]
		private static extern bool _AppLovinHasPreloadedInterstitial ();
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinPreloadInterstitial ();
	
		[DllImport ("__Internal")]
		private static extern bool _AppLovinIsInterstitialShowing ();
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetUnityAdListener (string gameObjectToNotify);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinLoadIncentInterstitial ();
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinShowIncentInterstitial ();

		[DllImport ("__Internal")]
		private static extern void _AppLovinSetIncentivizedUserName(string username);
	
		[DllImport ("__Internal")]
		private static extern bool _AppLovinIsIncentReady ();
	
		[DllImport ("__Internal")]
		private static extern bool _AppLovinIsCurrentInterstitialVideo ();

		[DllImport ("__Internal")]
		private static extern void _AppLovinTrackAnalyticEvent (string eventType, string serializedParameters);
	
	#endif
	
	#if UNITY_ANDROID
		public AndroidJavaClass applovinFacade = new AndroidJavaClass("com.applovin.sdk.unity.AppLovinFacade");
		public AndroidJavaObject currentActivity = null;
	#endif
	
	public static AppLovin DefaultPlugin = null;
	public AppLovinTargetingData targetingData = null;
	
	/**
	 * Gets the default AppLovin plugin
	 */
	public static AppLovin getDefaultPlugin() {
		if (DefaultPlugin == null) {
			#if UNITY_ANDROID
				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				DefaultPlugin = new AppLovin( jc.GetStatic<AndroidJavaObject>("currentActivity") );
			#else
				DefaultPlugin = new AppLovin();
			#endif
		}
		
		return DefaultPlugin;
	}
	
	
	/**
	 * Create an instance of AppLovin with a custom activity
	 * 
	 * @param {AndroidJavaObject} The activity that you are using
	 */
	#if UNITY_ANDROID
	public AppLovin(AndroidJavaObject activity) {
		if (activity == null) throw new MissingReferenceException("No parent activity specified");
		
		currentActivity = activity;
		targetingData = new AppLovinTargetingData(currentActivity);
	}
	#endif
	
	public AppLovin() {
		targetingData = new AppLovinTargetingData();	
	}
	
	/**
	 * Get AppLovinTargetingData object to set custom request parameters
	 */
	public AppLovinTargetingData getTargetingData() {
		return targetingData;
	}
	
	
	/**
	 * Manually initialize the SDK
	 */
	public void initializeSdk() {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("InitializeSdk", currentActivity);
		#endif
		#if UNITY_IPHONE
			_AppLovinInitializeSdk();
		#endif
	#endif
	}
	
	/**
	 * Loads and displays the AppLovin banner ad
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public void showAd() {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
		   applovinFacade.CallStatic("ShowAd", currentActivity);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinShowAd();	
		#endif
	#endif
	}
	
	
	/**
	 * Show an AppLovin Interstitial
	 */
	public void showInterstitial() {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
		   applovinFacade.CallStatic("ShowInterstitial", currentActivity);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinShowInterstitial();	
		#endif
	#endif
	}
	
	
	/**
	 * Hides the AppLovin ad
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public void hideAd() {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
		   applovinFacade.CallStatic("HideAd", currentActivity);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinHideAd();
		#endif
	#endif
	}
	
	
	/**
	 * Set the position of the banner ad
	 * 
	 * @param {float} x  Horizontal position of the ad (AD_POSITION_LEFT/RIGHT/CENTER)
	 * @param {float} y  Vertical position of the ad (AD_POSITION_TOP/BOTTOM)
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public void setAdPosition(float x, float y) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetAdPosition", currentActivity, x, y);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinSetAdPosition(x, y);
		#endif
	#endif
	}
	
	
	/**
	 * Set the width of the banner ad
	 * 
	 * @param {int} width  Desired width of the banner ad in dip
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public void setAdWidth(int width) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetAdWidth", currentActivity, width);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinSetAdWidth(width);	
		#endif
	#endif
	}
	
	public void setVerboseLoggingOn(string sdkKey) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetVerboseLoggingOn", "true");
		#endif
		
		#if UNITY_IPHONE
			_AppLovinSetVerboseLoggingOn("true");	
		#endif
	#endif
	}
	
	public void setSdkKey(string sdkKey) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetSdkKey", currentActivity, sdkKey);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinSetSdkKey(sdkKey);	
		#endif
	#endif
	}
	
	public void preloadInterstitial()
	{
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			 applovinFacade.CallStatic("PreloadInterstitial", currentActivity);
		#elif UNITY_IPHONE
			_AppLovinPreloadInterstitial();
		#endif
	#endif
	}
		
	/**
	 * Danger alert!
	 * 
	 * Native calls from Unity are extremely expensive. Polling this method in a loop will freeze your game!
	 * 
	 * We highly recommend you use an Ad Listener / callback flow rather than this synchronous flow.
	 * Due to Unity limitations, performance is significantly better via an asynchrounous flow!
	 */
	public bool hasPreloadedInterstitial() {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			string hasPreloadedAd = applovinFacade.CallStatic<string>("IsInterstitialReady", currentActivity);
			return System.Boolean.Parse(hasPreloadedAd);
		#elif UNITY_IPHONE
			return _AppLovinHasPreloadedInterstitial();
		#endif
	#endif
	return false;
	}
	
	/**
	 * Danger alert!
	 * 
	 * Native calls from Unity are extremely expensive. Polling this method in a loop will freeze your game!
	 * 
	 * We highly recommend you use an Ad Listener / callback flow rather than this synchronous flow.
	 * Due to Unity limitations, performance is significantly better via an asynchrounous flow!
	 */
	public bool isInterstitialShowing() {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			string isInterstitialShowing = applovinFacade.CallStatic<string>("IsInterstitialShowing", currentActivity);
			return System.Boolean.Parse(isInterstitialShowing);
		#elif UNITY_IPHONE
			return _AppLovinIsInterstitialShowing();
		#endif
	#endif
	return false;
	}
	
	public void setAdListener(string gameObjectToNotify)
	{	
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetUnityAdListener", gameObjectToNotify);
		#elif UNITY_IPHONE
			_AppLovinSetUnityAdListener(gameObjectToNotify);
		#endif
	#endif
	}	
	
	public void setRewardedVideoUsername(string username)
	{	
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetIncentivizedUsername", currentActivity, username);
		#elif UNITY_IPHONE
			_AppLovinSetIncentivizedUserName(username);
		#endif
	#endif
	}
	
	public void loadIncentInterstitial()
	{
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			 applovinFacade.CallStatic("LoadIncentInterstitial", currentActivity);
		#elif UNITY_IPHONE
			_AppLovinLoadIncentInterstitial();
		#endif
	#endif
	}
	
	
	public void showIncentInterstitial()
	{
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			 applovinFacade.CallStatic("ShowIncentInterstitial", currentActivity);
		#elif UNITY_IPHONE
			_AppLovinShowIncentInterstitial();
		#endif
	#endif
	}
	
	/**
	 * Danger alert!
	 * 
	 * Native calls from Unity are extremely expensive. Polling this method in a loop will freeze your game!
	 * 
	 * We highly recommend you use an Ad Listener / callback flow rather than this synchronous flow.
	 * Due to Unity limitations, performance is significantly better via an asynchrounous flow!
	 */
	public bool isIncentInterstitialReady()
	{
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			string isReady = applovinFacade.CallStatic<string>("IsIncentReady", currentActivity);
			return System.Boolean.Parse(isReady);
		#elif UNITY_IPHONE
			return _AppLovinIsIncentReady();
		#endif
	#endif	
		return false;
	}
		
	/**
	 * Danger alert!
	 * 
	 * Native calls from Unity are extremely expensive. Polling this method in a loop will freeze your game!
	 * 
	 * We highly recommend you use an Ad Listener / callback flow rather than this synchronous flow.
	 * Due to Unity limitations, performance is significantly better via an asynchrounous flow!
	 */
	public bool isPreloadedInterstitialVideo()
	{
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			string isVideo = applovinFacade.CallStatic<string>("IsCurrentInterstitialVideo", currentActivity);
			return System.Boolean.Parse(isVideo);
		#elif UNITY_IPHONE
			return _AppLovinIsCurrentInterstitialVideo();
		#endif
	#endif
		return false;
	}


	/**
	 * Track an event of a given type.
	 *
	 * @param eventType A string describing this event; can one of the predefined AppLovinEvents.Types constants defined in AppLovinEvents.cs, or a custom string.
	 * @param parameters A dictionary containing key-value pairs further describing this event. Particular data points of interest are provided as "suggested keys" in the doc comment for each event type constant in AppLovinEvents.cs.
	 */
	public void trackEvent(string eventType, IDictionary <string, string> parameters)
	{
	#if !UNITY_EDITOR
		System.Text.StringBuilder serializedParameters = new System.Text.StringBuilder();

		if (parameters != null)
		{
			foreach (KeyValuePair <string, string> entry in parameters)
			{
				if (entry.Key != null && entry.Value != null)
				{
					serializedParameters.Append (entry.Key);
					serializedParameters.Append (_InternalSecondarySeparator);
					serializedParameters.Append (entry.Value);
					serializedParameters.Append (_InternalPrimarySeparator);
				}
			}
		}
		#if UNITY_ANDROID
			applovinFacade.CallStatic("TrackEvent", currentActivity, eventType, serializedParameters.ToString());
		#elif UNITY_IPHONE
			_AppLovinTrackAnalyticEvent(eventType, serializedParameters.ToString());
		#endif
	#endif
	}

	public void enableImmersiveMode()
	{	
	#if UNITY_ANDROID
		applovinFacade.CallStatic("EnableImmersiveMode", currentActivity);
	#elif UNITY_IPHONE
		// Immersive mode does not exist on iOS. Nothing to do.
	#endif
	}
	
	/**
	 * Loads and displays the AppLovin banner ad
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public static void ShowAd() {
		getDefaultPlugin().showAd();
	}
	
	/**
	 * Loads and displays the AppLovin banner ad at given position
	 * 
	 * @param {float} x  Horizontal position of the ad (AD_POSITION_LEFT, AD_POSITION_CENTER, AD_POSITION_RIGHT) or float
	 * @param {float} y  Vertical position of the ad (AD_POSITION_TOP, AD_POSITION_BOTTOM) or float
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public static void ShowAd(float x, float y) {
		AppLovin.SetAdPosition (x, y);
		AppLovin.ShowAd ();
	}
	
	
	/**
	 * Show an AppLovin Interstitial
	 */
	public static void ShowInterstitial() {
		getDefaultPlugin().showInterstitial();
	}
	
	/* Preload/show an AppLovin rewarded interstitial.
	 * Ideally, you should set an ad listener to
	 * be notified as to the result of the reward. */
	
	public static void LoadRewardedInterstitial()
	{
		getDefaultPlugin().loadIncentInterstitial();
	}
	
	public static void ShowRewardedInterstitial()
	{
		getDefaultPlugin().showIncentInterstitial();
	}
	
	/**
	 * Hides the AppLovin ad
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public static void HideAd() {
		getDefaultPlugin().hideAd();
	}
	
	/**
	 * Set the position of the banner ad
	 * 
	 * @param {float} x  Horizontal position of the ad (AD_POSITION_LEFT, AD_POSITION_CENTER, AD_POSITION_RIGHT) or float
	 * @param {float} y  Vertical position of the ad (AD_POSITION_TOP, AD_POSITION_BOTTOM) or float
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public static void SetAdPosition(float x, float y) {
		getDefaultPlugin().setAdPosition(x, y);
	}

	/**
	 * Set the width of the banner ad
	 * 
	 * @param {int} width  Desired width of the banner ad in dip
	 */
	[System.Obsolete("Banners are deprecated and will be removed in a future SDK version. Consider using interstitials or rewarded videos instead.")]
	public static void SetAdWidth(int width) {
		getDefaultPlugin().setAdWidth(width);
	}

	/**
	 * Set the SDK key to be used.
	 */
	public static void SetSdkKey(string sdkKey) {
		getDefaultPlugin().setSdkKey(sdkKey);
	}

	/**
	 * Set verbose logging on or off. Pass the string "true" or "false".
	 */
	public static void SetVerboseLoggingOn(string verboseLogging) {
		getDefaultPlugin().setVerboseLoggingOn(verboseLogging);
	}
	
	/**
	 * Get AppLovinTargetingData object to set custom request parameters
	 */
	public static AppLovinTargetingData GetTargetingData() {
		return getDefaultPlugin().getTargetingData();
	}
	
	public static void PreloadInterstitial()
	{
		getDefaultPlugin().preloadInterstitial();
	}
	
	/**
	 * Danger alert!
	 * 
	 * Native calls from Unity are extremely expensive. Polling this method in a loop will freeze your game!
	 * 
	 * We highly recommend you use an Ad Listener / callback flow rather than this synchronous flow.
	 * Due to Unity limitations, performance is significantly better via an asynchrounous flow!
	 */
	public static bool HasPreloadedInterstitial()
	{
		return getDefaultPlugin().hasPreloadedInterstitial();
	}
	
	/**
	 * Danger alert!
	 * 
	 * Native calls from Unity are extremely expensive. Polling this method in a loop will freeze your game!
	 * 
	 * We highly recommend you use an Ad Listener / callback flow rather than this synchronous flow.
	 * Due to Unity limitations, performance is significantly better via an asynchrounous flow!
	 */
	public static bool IsInterstitialShowing()
	{
		return getDefaultPlugin().isInterstitialShowing();
	}
	
	/**
	 * Danger alert!
	 * 
	 * Native calls from Unity are extremely expensive. Polling this method in a loop will freeze your game!
	 * 
	 * We highly recommend you use an Ad Listener / callback flow rather than this synchronous flow.
	 * Due to Unity limitations, performance is significantly better via an asynchrounous flow!
	 */
	public static bool IsIncentInterstitialReady()
	{
		return getDefaultPlugin().isIncentInterstitialReady();
	}
	
	
	/**
	 * Check if a currently preloaded interstitial is a video ad or not.
	 */
	public static bool IsPreloadedInterstitialVideo()
	{
		return getDefaultPlugin().isPreloadedInterstitialVideo();
	}
	
	// Initialize the AppLovin SDK
	public static void InitializeSdk()
	{
	    getDefaultPlugin().initializeSdk();
	}
	
	/* New in plugin 3.0.0 is the ability to set an Ad Listener.
	 * You provide us with the name of a GameObject to call when
	 * certain ad events happen - including ads loading and failing
	 * to load, ads being displayed and closed, videos starting and
	 * stopping, and even video rewards being approved and rejected.
	 * 
	 * When you provide us this game object, you need to ensure it
	 * has a method attached to it with the following signature:
	 * 
	 * public void onAppLovinEventReceived(string event) {}
	 * 
	 * We will call this method with any of the following event strings:
	 * 
	 * LOADED - An ad was loaded by the AppLovin plugin.
	 * LOADFAILED - An ad failed to load.
	 * DISPLAYED - An ad was attached to the window.
	 * HIDDEN - An ad was removed from the window.
	 * CLICKED - The user clicked the ad.
	 * VIDEOBEGAN - Video playback started.
	 * VIDEOSTOPPED - Video playback stopped.
	 * REWARDAPPROVED - A rewarded video finished & was approved, refresh the user's coins from your server.
	 * REWARDOVERQUOTA - A user already received the maximum amount of rewards.
	 * REWARDREJECTED - AppLovin rejected the reward, possibly due to fraud.
	 * REWARDTIMEOUT - AppLovin couldn't contact the reward server.
	 */
	
	public static void SetUnityAdListener(string gameObjectToNotify)
	{
		getDefaultPlugin().setAdListener(gameObjectToNotify);
	}
	
	public static void SetRewardedVideoUsername(string username)
	{
		getDefaultPlugin().setRewardedVideoUsername(username);
	}
	
	/**
	 * Track an event of a given type.
	 *
	 * @param eventType A string describing this event; can one of the predefined AppLovinEvents.Types constants defined in AppLovinEvents.cs, or a custom string.
	 * @param parameters A dictionary containing key-value pairs further describing this event. Particular data points of interest are provided as "suggested keys" in the doc comment for each event type constant in AppLovinEvents.cs.
	 */
	public static void TrackEvent(string eventType, IDictionary <string, string> parameters)
	{
		getDefaultPlugin().trackEvent (eventType, parameters);
	}

	/**
	 * Hide the status bar and soft keys on relevant Android devices, e.g. Nexus devices.
	 */
	public static void EnableImmersiveMode()
	{
		getDefaultPlugin().enableImmersiveMode();
	}
}

public class AppLovinTargetingData {
	public const string GENDER_MALE 				= "m";
	public const string GENDER_FEMALE 				= "f";
	
	#if UNITY_IPHONE
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetGender (string gender);
		
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetBirthYear (int birthYear);
		
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetLanguage(string language);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetCountry(string country);
		
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetCarrier(string carrier);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetInterests(string[] interests);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinSetKeywords(string[] keywords);
	
		[DllImport ("__Internal")]
		private static extern void _AppLovinPutExtra(string key, string val);
	
	#endif
	
	public AppLovinTargetingData() {
		// default constructor
	}
	
	#if UNITY_ANDROID
		public AndroidJavaObject currentActivity = null;
		public AndroidJavaClass applovinFacade = new AndroidJavaClass("com.applovin.sdk.unity.AppLovinFacade");
	
		public AppLovinTargetingData(AndroidJavaObject activity) {
			if (activity == null) throw new MissingReferenceException("No parent activity specified");

			currentActivity = activity;
		}
	#endif

	/**
	 * Set the current user's gender
	 * 
	 * @param {string} gender  The current user's gender
	 */
	public void setGender(string gender) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetGender", currentActivity, gender);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinSetGender(gender);
		#endif
	#endif
	}
	
	/**
	 * Set the current user's year of birth
	 * 
	 * @param {int} birthYear  The current user's year of birth
	 */
	public void setBirthYear(int birthYear) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetBirthYear", currentActivity, birthYear);
		#endif
				
		#if UNITY_IPHONE
			_AppLovinSetBirthYear(birthYear);
		#endif
	#endif
	}
	
	/**
	 * Set the current user's language
	 * 
	 * @param {string} language  The current user's language
	 */
	public void setLanguage(string language) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetLanguage", currentActivity, language);
		#endif
				
		#if UNITY_IPHONE
			_AppLovinSetLanguage(language);
		#endif
	#endif
	}
	
	/**
	 * Set the current user's country
	 * 
	 * @param {string} country  The current user's country
	 */
	public void setCountry(string country) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetCountry", currentActivity, country);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinSetCountry(country);
		#endif
	#endif
	}
	
	/**
	 * Set the current user's carrier
	 * 
	 * @param {string} carrier  The current user's carrier
	 */
	public void setCarrier(string carrier) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetCarrier", currentActivity, carrier);
		#endif

		#if UNITY_IPHONE
			_AppLovinSetCarrier(carrier);
		#endif
	#endif
	}
	
	/*
	 * Set the current user's interests
	 * 
	 * @param {params string[]} interests  The current user's interests
	 */
	public void setInterests(params string[] interests) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetInterests", currentActivity, interests);
		#endif

		#if UNITY_IPHONE
			_AppLovinSetInterests(interests);
		#endif
	#endif
	}
	
	/**
	 * Set the keywords for the current app
	 * 
	 * @param {params string[]} keywords  Some keywords for the current app
	 */
	public void setKeywords(params string[] keywords) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("SetKeywords", currentActivity, keywords);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinSetKeywords(keywords);
		#endif
	#endif
	}
	
	/**
	 * Set extra parameter for ad requests
	 * 
	 * @param {string} key  Key of the parameter
	 * @param {string} val  Value of the parameter
	 */
	public void putExtra(string key, string val) {
	#if !UNITY_EDITOR
		#if UNITY_ANDROID
			applovinFacade.CallStatic("PutExtra", currentActivity, key, val);
		#endif
		
		#if UNITY_IPHONE
			_AppLovinPutExtra(key, val);
		#endif
	#endif
	}
}

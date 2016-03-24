AppLovin Unity Plugin 3.2.0

https://www.applovin.com/

================


- Getting Started -


Android -

The first thing you need to do is edit the AndroidManifest file and put your AppLovin SDK key where it says YOUR_SDK_KEY.

The second thing you need to do is replace all instances of "YOUR_PACKAGE_NAME" with your application's package name.


iOS -

You need to call AppLovin.setSdkKey("YOUR_SDK_KEY") or set AppLovinSdkKey to your SDK Key in your applications info.plist every time after compiling from Unity.


Both -

We recommend you call AppLovin.InitializeSdk() before calling any of the showAd/showInterstitial methods.
This will allow the SDK to perform initial start-up tasks like pre-caching the first ad, resulting in
a more responsive initial ad display.

---------------------------


- Plugin Usage -


Using the AppLovin C# wrapper class and the AppLovinFacade java class coupled with the iOS native plugin, 
you can easily manipulate Ads programmatically across both platforms.


---------------------------


Show Banner Ad:

	AppLovin.ShowAd();


---------------------------


Show Banner Ad at Position:

With constants -

	AppLovin.ShowAd(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_BOTTOM);

Available horizontal constants are: AD_POSITION_CENTER, AD_POSITION_LEFT, AD_POSITION_RIGHT
Available vertical constants are: AD_POSITION_TOP, AD_POSITION_BOTTOM

With dp -

	AppLovin.ShowAd("50", "50");

---------------------------


Hide the Ad:

	AppLovin.HideAd();


---------------------------


Interstitial Ad:

	AppLovin.ShowInterstitial();


---------------------------


Update Ad Position:

With constants -

	AppLovin.SetAdPosition(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_BOTTOM);

Available horizontal constants are: AD_POSITION_CENTER, AD_POSITION_LEFT, AD_POSITION_RIGHT
Available vertical constants are: AD_POSITION_TOP, AD_POSITION_BOTTOM

With dp -

	AppLovin.SetAdPosition("50", "50");

---------------------------


Set Ad Width:

	AppLovin.SetAdWidth(400);

The width is in dip.


---------------------------

For more information, including ad listeners, rewarded videos,
and other advanced features, please see the online documentation
at: http://applovin.com/integration.

---------------------------



If you have any questions regarding the Unity Plugin, contact AppLovin support at support@applovin.com

https://www.applovin.com/
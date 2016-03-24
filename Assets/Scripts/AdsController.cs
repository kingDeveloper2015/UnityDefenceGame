using UnityEngine;
using System.Collections;

public class AdsController : MonoBehaviour {

    public static AdsController instance;
    
     void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            
            AppLovin.SetSdkKey("kGJHOuC0iJRstnpg62rWosOVBLPvIlt10BIO4VOnqrchbQMhSCDF3bmEyA7Fv_GvAZIE3h0tWdT72wqU71lBcU");
            AppLovin.InitializeSdk();

        }
        else
        {
            Destroy(gameObject);
        }
    }
	

    public void ShowTopBanner()
    {
        HideTopBanner();
        AppLovin.ShowAd(AppLovin.AD_POSITION_CENTER, AppLovin.AD_POSITION_TOP);
    }

    public void HideTopBanner()
    {
        AppLovin.HideAd();
    }

    public void ShowInterstitial()
    {
        AppLovin.ShowInterstitial();
    }
}

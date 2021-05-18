using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;
//using admob;
using UnityEngine.UI;

public class AdManager : MonoBehaviour {

    public static AdManager instance;

    [SerializeField]
    private string bannerID = "";
    [SerializeField]
    private string videoID = "";

    void Awake()
    {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        //For detailed info check this link: https://www.youtube.com/watch?v=khlROw-PfNE
        //here we initialize admob
        #if UNITY_EDITOR
        Debug.Log("Working");
#elif UNITY_ANDROID
        //Admob.Instance().initAdmon(bannerID,videoID);
        //Admob.Instance().setTesting(true); //use this code only for testing time
        //Admob.Instance().loadInterstitial();
#endif
    }

    // Update is called once per frame
    void Update () {
	
	}

    //                                                                ......................UNITY_ADS
    //public IEnumerator ShowAdsButton()
    //{
    //    while (!Advertisement.IsReady("rewardedVideo"))
    //        yield return null;
    //    if (InGameGui.instance != null)
    //        MainMenuManager.instance.adsButton.interactable = false;

    //}

    //use this for ads button
    public void ShowRewardedAd()
    {
        //if (Advertisement.IsReady("rewardedVideo"))
        //{
        //    MainMenuManager.instance.adsButton.interactable = true;

        //    var options = new ShowOptions { resultCallback = HandleShowResult };
        //    Advertisement.Show("rewardedVideo", options);
        //}
    }

    //private void HandleShowResult(ShowResult result)
    //{
    //    switch (result)
    //    {
    //        case ShowResult.Finished:
    //            Debug.Log("The ad was successfully shown.");
    //            GameController.instance.juice += 20;
    //            GameController.instance.Save();
    //            break;
    //        case ShowResult.Skipped:
    //            Debug.Log("The ad was skipped before reaching the end.");
    //            break;
    //        case ShowResult.Failed:
    //            Debug.LogError("The ad failed to be shown.");
    //            break;
    //    }
    //}
    //                                                             ..................UNITY_ADS


    //..........................................................................Admob
    public void ShowBanner()
    {
#if UNITY_EDITOR
        Debug.Log("Working");
#elif UNITY_ANDROID
        //Admob.Instance().showBannerRelative(AdSize.Banner,AdPosition.TOP_CENTER , 5);
#endif
    }

    public void ShowVideo()
    {
#if UNITY_EDITOR
        Debug.Log("Working");
#elif UNITY_ANDROID
        /*
        if (Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().showInterstitial();
        }
        */
#endif
    }

}

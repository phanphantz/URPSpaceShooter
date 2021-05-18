using UnityEngine;
using System.Collections;
//using GooglePlayGames;
//using UnityEngine.SocialPlatforms;

public class GooglePlayServiceManager : MonoBehaviour {

    public static GooglePlayServiceManager instance;

    [SerializeField] private string LEADERBOARDS_SCORE = "CgkI4MTMtpUQEAIQAA";//menu button not included you can add yours
    [SerializeField]
    private static string Master = "CgkI4MTMtpUQEAIQAQ"; //change this keys depending on yours
    [SerializeField]
    private static string Pro = "CgkI4MTMtpUQEAIQAg";
    [SerializeField]
    private static string God = "CgkI4MTMtpUQEAIQAw";
    [SerializeField]
    private static string UnlockMaster = " CgkI4MTMtpUQEAIQBA";
    [SerializeField]
    private static string GameLeader = "CgkI4MTMtpUQEAIQBQ";

    private string[] achievements_names = { Master, Pro, God, UnlockMaster, GameLeader };

    private bool[] achievements;

    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
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

    void InitializeAchievements()
    {
        achievements = GameController.instance.achievements;

        for (int i = 0; i < achievements.Length; i++)
        {
            if (!achievements[i])
            {
                //achivement changes
                Social.ReportProgress(achievements_names[i], 0.0f, (bool success) => {
                    //handle success
                });
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        //    PlayGamesPlatform.Activate();
        //    Social.localUser.Authenticate((bool success) =>
        //    {
        //        if (success)
        //        {
        //            InitializeAchievements();
        //        }
        //    });
    }

    void OnLevelWasLoaded()
    {
        //CheckIfAnyUnlockedAchievements();
        //ReportScore(GameController.instance.hiScore);
    }

    //use for button
    public void OpenLeaderboardsScore()
    {
        //if (Social.localUser.authenticated)
        //{
        //    PlayGamesPlatform.Instance.ShowLeaderboardUI(LEADERBOARDS_SCORE);
        //}
    }

    void ReportScore(int score)
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, LEADERBOARDS_SCORE, (bool success) => { });
        }
    }

    //use for button
    public void OpenAchievements()
    {
        if (Social.localUser.authenticated)
        {
            Social.ShowAchievementsUI();
        }
    }

    //void UnlockAchievements(int index)
    //{
    //    if (Social.localUser.authenticated)
    //    {
    //        //achievement change
    //        Social.ReportProgress(achievements_names[index], 100.0f, (bool success) =>
    //        {
    //            if (success)
    //            {
    //                achievements[index] = true;
    //                GameController.instance.achievements = achievements;
    //                GameController.instance.Save();
    //            }
    //        });
    //    }
    //}

    //void CheckIfAnyUnlockedAchievements()
    //{
    //    we check if GameController is present
    //    if (GameController.instance != null)
    //        {
    //            then we check if our score is greater than of equal to 20
    //        if (GameController.instance.currentScore >= 20)
    //            {
    //                then we check if our 1st achievement is unlocke or not
    //            if (!achievements[0])
    //                {
    //                    if not then we check if player is logged in
    //                if (Social.localUser.authenticated)
    //                    {
    //                        then we unlock the achievement
    //                        UnlockAchievements(0);
    //                    }
    //                }
    //            }
    //        }//Achievement 0


    //    we check if GameController is present
    //    if (GameController.instance != null)
    //        {
    //            then we check if our score is greater than of equal to required score
    //        if (GameController.instance.currentScore >= 45)
    //            {
    //                then we check if our 2nd achievement is unlocke or not
    //            if (!achievements[1])
    //                {
    //                    if not then we check if player is logged in
    //                if (Social.localUser.authenticated)
    //                    {
    //                        then we unlock the achievement
    //                        UnlockAchievements(1);
    //                    }
    //                }
    //            }
    //        }//Achievement 1


    //    we check if GameController is present
    //    if (GameController.instance != null)
    //        {
    //            then we check if our score is greater than of equal to required score
    //        if (GameController.instance.currentScore >= 80)
    //            {
    //                then we check if our 3rd achievement is unlocke or not
    //            if (!achievements[2])
    //                {
    //                    if not then we check if player is logged in
    //                if (Social.localUser.authenticated)
    //                    {
    //                        then we unlock the achievement
    //                        UnlockAchievements(2);
    //                    }
    //                }
    //            }
    //        }//Achievement 2


    //    we check if GameController is present
    //    if (GameController.instance != null)
    //        {
    //            we check if 1st 2 ships are unlocked
    //        if (GameController.instance.players[1] == true && GameController.instance.players[2] == true)
    //            {
    //                then we check if our 4th achievement is unlocke or not
    //            if (!achievements[3])
    //                {
    //                    if not then we check if player is logged in
    //                if (Social.localUser.authenticated)
    //                    {
    //                        then we unlock the achievement
    //                        UnlockAchievements(3);
    //                    }
    //                }
    //            }

    //        }//Achievement 3


    //    we check if GameController is present
    //    if (GameController.instance != null)
    //        {
    //            then we check if all ships are unlocked
    //        if (GameController.instance.players[1] == true && GameController.instance.players[2] == true
    //            && GameController.instance.players[3] == true)
    //            {
    //                then we check if our 5th achievement is unlocke or not
    //            if (!achievements[4])
    //                {
    //                    if not then we check if player is logged in
    //                if (Social.localUser.authenticated)
    //                    {
    //                        then we unlock the achievement
    //                        UnlockAchievements(4);
    //                    }
    //                }
    //            }
    //        }//Achievement 4



    //}//CheckIfAnyUnlockedAchievements

}

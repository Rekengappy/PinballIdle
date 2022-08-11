using System;
using Controllers;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Mycom.Tracker.Unity;


namespace Managers
{
    public class AnalyticManager : MonoBehaviour
    {

        public static bool isMyTrackerInit;
        public static bool isRemoteInit;
        private void Awake()
        {
#if !UNITY_IOS && !UNITY_ANDROID
        return;
#endif
            // ...
            // Настройте параметры трекера
            // ...

            // Инициализируйте трекер в зависимости от платформы
#if !UNITY_EDITOR
            MyTracker.Init("94020374764550666248");
            isMyTrackerInit=true;
#endif
            Debug.Log("Awake");
        }

        // Start is called before the first frame update
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitFirebase()
        {
            Debug.Log("Start Init");
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                var _dependencyStatus = task.Result;
                if (_dependencyStatus == DependencyStatus.Available)
                {
                    try
                    {
                        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero)
                            .ContinueWithOnMainThread((fetchTask) =>
                            {
                                if (fetchTask.IsFaulted)
                                {
                                    Debug.LogError(fetchTask.Exception);
                                }

                                try
                                {
                                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                                        .ActivateAsync() /*.SetDefaultsAsync(GetDefault())*/
                                        .ContinueWithOnMainThread(task1 =>
                                        {
                                            if (task1.IsCompleted)
                                            {
                                            }

                                            if (task1.IsFaulted)
                                            {
                                                Debug.LogError(task1.Exception);
                                            }

                                            isRemoteInit = true;
                                            Debug.Log(
                                                $"Firebase config last fetch time {Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info.FetchTime}.");
                                            IaPurchase.NumberOfPrice = (int)GetLong("Price");
                                            Debug.Log("This price "+IaPurchase.NumberOfPrice);

                                        });
                                }
                                catch (Exception _e)
                                {
                                    Debug.LogError(_e.Message);
                                }
                            });
                    }
                    catch (Exception _e)
                    {
                        Debug.LogError("Remote Config exception " + _e.Message);
                    }
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
                    Application.Quit();
                }
            });

            Debug.Log("End Init");
        }

        public static bool GetBool(string configName)
        {
            try
            {
                return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(configName).BooleanValue;
            }
            catch (Exception _e)
            {
                Debug.Log("Remote config error. " + _e.Message);
                return true;
            }
        }

        public static long GetLong(string configName)
        {
            try
            {
                return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(configName).LongValue;
            }
            catch (Exception _e)
            {
                Debug.Log("Remote config error. " + _e.Message + ".." + _e.StackTrace);
                return 0;
            }
        }

        public static double GetDouble(string configName)
        {
            try
            {
                return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(configName).DoubleValue;
            }
            catch (Exception _e)
            {
                Debug.Log("Remote config error. " + _e.Message + ".." + _e.StackTrace);
                return 0f;
            }
        }

        public static string GetString(string configName)
        {
            try
            {
                return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(configName).StringValue;
            }
            catch (Exception _e)
            {
                Debug.Log("Remote config error. " + _e.Message + ".." + _e.StackTrace);
                return "";
            }
        }

        public static void StartChallenge()
        {
            FirebaseAnalytics.LogEvent("start_challenge");
        }

        public static void CompleteChallenge()
        {
            FirebaseAnalytics.LogEvent("complete_challenge");
            MyTracker.TrackEvent("complete_challenge");
        }

        public static void OpenDonateShop()
        {
            FirebaseAnalytics.LogEvent("open_donate_shop");
            MyTracker.TrackEvent("open_donate_shop");
        }

        public static void OpenNewField()
        {
            FirebaseAnalytics.LogEvent("open_new_field");
            
            MyTracker.TrackEvent("open_new_field");
        }

        public static void OpenNewBall()
        {
            FirebaseAnalytics.LogEvent("open_new_ball");
            MyTracker.TrackEvent("open_new_ball");
        }

        public static void OpenStatistic()
        {
            FirebaseAnalytics.LogEvent("statistic");
            MyTracker.TrackEvent("statistic");
        }
        public static void BuyCoinForDiamond()
        {
            FirebaseAnalytics.LogEvent("buy_coin_for_diamond");
            MyTracker.TrackEvent("buy_coin_for_diamond");
        }

        public static void ChangeTheme()
        {
            FirebaseAnalytics.LogEvent("change_theme");
            MyTracker.TrackEvent("change_theme");
        }
        public static void StartCompetition()
        {
            FirebaseAnalytics.LogEvent("start_competition");
            MyTracker.TrackEvent("start_competition");
        }
        public static void FirstPlaceCompetition()
        {
            FirebaseAnalytics.LogEvent("first_place_competition");
            MyTracker.TrackEvent("first_place_competition");
        }
        public static void SecondPlaceCompetition()
        {
            FirebaseAnalytics.LogEvent("second_place_competition");
            MyTracker.TrackEvent("second_place_competition");
        }
        public static void ThirdPlaceCompetition()
        {
            FirebaseAnalytics.LogEvent("third_place_competition");
            MyTracker.TrackEvent("third_place_competition");
        }
        public static void LoseCompetition()
        {
            FirebaseAnalytics.LogEvent("lose_competition");
            MyTracker.TrackEvent("lose_competition");
        }
        public static void ReviewWasShow()
        {
            FirebaseAnalytics.LogEvent("review_was_show");
            MyTracker.TrackEvent("review_was_show");
        }
    }
}
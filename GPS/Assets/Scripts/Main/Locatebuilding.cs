using UnityEngine;
using System.Collections;
using System.IO;
using System;
using LitJson;
using System.Collections.Generic;
using OneSignalPush.MiniJSON;

public class Locatebuilding : MonoBehaviour {
    // 0: gps off, 1 : gps on(service out) , 2 : gps on & service in
    void Start () {
        //start the main routine
        #if UNITY_ANDROID
        if(Input.location.status == LocationServiceStatus.Stopped)
        Input.location.Start(10, 0.1f);
        #elif UNITY_IPHONE
        Input.location.Start ();
        #endif

        OneSignal.StartInit(Global.onesignal_appid)
        .HandleNotificationReceived(HandleNotificationReceived)
        .HandleNotificationOpened(HandleNotificationOpened)
        .EndInit();
        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
    }

    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {
    }

    private static void HandleNotificationReceived(OSNotification notification)
    {
        OSNotificationPayload payload = notification.payload;
        string message = payload.body;
    }

    private static string oneSignalDebugMessage;

    void someMethod()
    {
        OneSignal.IdsAvailable((userId, pushToken) =>
        {
            if (pushToken != null)
            {
                // Just an example userId, use your own or get it the devices by using the GetPermissionSubscriptionState method
                var notification = new Dictionary<string, object>();
                notification["contents"] = new Dictionary<string, string>() { { "en", Global.push_string } };
                notification["include_player_ids"] = new List<string>() { Global.onesignal_userid };
                // Example of scheduling a notification in the future.
                notification["send_after"] = System.DateTime.Now.ToUniversalTime().AddSeconds(1).ToString("U");
                OneSignal.PostNotification(notification, (responseSuccess) => {
                    oneSignalDebugMessage = "Notification posted successful!\n" + Json.Serialize(responseSuccess);
                }, (responseFailure) => {
                    oneSignalDebugMessage = "Notification failed to post:\n" + Json.Serialize(responseFailure);
                });
            }
        });
    }

    private void Awake()
    {
        iOSLocalNotification.RegisterNotificationSettings();
    }

    private float timer = 0.0f;
    //private float test_timer = 0.0f;
    public void Update()
    {
        if (Global.marker_set)
        {
            timer += Time.deltaTime;
            //test_timer += Time.deltaTime;
            if (timer > 5.0f)
            {
                timer = 0.0f;
                if (Input.location.status != LocationServiceStatus.Running)
                {
                    Global.curPos = Global.defaultPos;
                    Global.push_string = "";
                    SetNotification("La funcio de GPS no funciona en este teleono ahora.", 5);
                }
                else
                {
                    Global.push_string = "";
                    Global.curPos.latitude = Input.location.lastData.latitude;
                    Global.curPos.longitude = Input.location.lastData.longitude;
                    Global.gps_set = true;
                    for (int i = 0; i < Global.markers.Count; i++)
                    {
                        int d = (int)OnlineMapsUtils.DistanceBetweenPointsD_GRS(Global.curPos.latitude, Global.curPos.longitude, Global.markers[i].latitude, Global.markers[i].longitude);
                        if (d < Global.dis && !Global.isArr[i] && Global.markers[i].status)
                        {
                            Global.push_string += Global.markers[i].name + " - Located at " + d.ToString() + " meters offers you today " + Global.markers[i].offerofday + "\n";
                            Global.isArr[i] = true;
                        }
                        else if (d > Global.dis)
                        {
                            Global.isArr[i] = false;
                        }
                        if (Global.markers[i].status)
                        {
                            Global.info_string[i] = "Located at " + d.ToString() + " meters - " + Global.markers[i].offerofday;
                        }
                    }

                    if (Global.push_string != "" && Global.isset_push)
                    {
                        SetNotification(Global.push_string, 5);
                        someMethod();
                    }else if (!Global.isset_push)
                    {
                        //SetNotification("not available push notification", 5);
                    }
                    else
                    {
                        /*
                        if(test_timer > 60)
                        {
                            SetNotification("not arriving at any defined positions", 5);
                            Global.push_string = "Not arriving at any defined position";
                            someMethod();
                            test_timer = 0;
                        }
                        */
                    }
                }
            }
        }
    }
    
    public bool CheckNotificationRegistered()
    {
        if (iOSLocalNotification.IsNotificationPermitted())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void SetNotification(string text, int seconds)
    {
        if (!CheckNotificationRegistered())
        {
            iOSLocalNotification.RegisterNotificationSettings();
        }
        iOSLocalNotification.SetNotification(text, seconds);
    }
}


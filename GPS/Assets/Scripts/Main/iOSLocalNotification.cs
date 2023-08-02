using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using LocalNotification = UnityEngine.iOS.LocalNotification;
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;

public class iOSLocalNotification {
    [DllImport("__Internal")]
    static extern void CleanIconBadge_();

    [DllImport("__Internal")]
    static extern void OpenAppSettingPage_();

    [DllImport("__Internal")]
    static extern bool IsNotificationPermitted_();

    private static LocalNotification l = new LocalNotification();
    public static void OpenAppSettingPage()
    {
        OpenAppSettingPage_();
    }

    public static void CleanIconBadge()
    {
        CleanIconBadge_();
    }

    public static bool IsNotificationPermitted()
    {
        return IsNotificationPermitted_();
    }

    public static void SetNotification(string message, int delayTime, int badgeNumber = 1)
    {
        //var l = new LocalNotification();
        l.applicationIconBadgeNumber = badgeNumber;
        l.fireDate = System.DateTime.Now.AddSeconds(delayTime);
        l.alertBody = message;
        NotificationServices.ScheduleLocalNotification(l);
    }

    public static void RegisterNotificationSettings()
    {
        NotificationServices.RegisterForNotifications(
            NotificationType.Alert |
            NotificationType.Badge |
            NotificationType.Sound);
    }

    public static void CancelAllNotifications()
    {
        NotificationServices.CancelAllLocalNotifications();
    }
}

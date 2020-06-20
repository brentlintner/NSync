using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Notifications.Management;
using System.Collections.Generic;
using Windows.UI.Notifications;

class App
{
    private UserNotificationListener listener;
    private List<uint> notifications;

    public App()
    {
        this.notifications = new List<uint>();
    }

    public async Task<bool> RequestAccess()
    {
        if (!await this.RequestListenerAccess())
        {
            Console.WriteLine("Listener access denied.");
            return false;
        }

        Console.WriteLine("Access Granted.");
        return true;
    }

    public async void SyncNotifications()
    {
        IReadOnlyList<UserNotification> userNotifications = await this.listener.GetNotificationsAsync(NotificationKinds.Toast);

        foreach (UserNotification userNotification in userNotifications)
        {
            if (!this.notifications.Contains(userNotification.Id))
            {
                // send HttpClient request of new notification
                this.notifications.Add(userNotification.Id);
                this.listener.RemoveNotification(userNotification.Id);
            }
        }
    }

    private async Task<bool> RequestListenerAccess()
    {
        this.listener = UserNotificationListener.Current;

        UserNotificationListenerAccessStatus accessStatus = await this.listener.RequestAccessAsync();

        switch (accessStatus)
        {
            case UserNotificationListenerAccessStatus.Allowed:
                return true;

            case UserNotificationListenerAccessStatus.Denied:
            case UserNotificationListenerAccessStatus.Unspecified:
            default:
                return false;
        }
    }
}


// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace NSync
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("Hello - no args");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"arg[{i}] = {args[i]}");
                }
            }
            Console.WriteLine("Press a key to continue: ");
            Console.ReadLine();

            var App = new App();

            await App.RequestAccess();

            while (true)
            {
                App.SyncNotifications();
                Thread.Sleep(5000);
            }
        }
    }
}
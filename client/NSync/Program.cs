using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Notifications.Management;
using System.Collections.Generic;
using Windows.UI.Notifications;
using Windows.Foundation.Metadata;
using System.Runtime.CompilerServices;

namespace NSync
{
    class Program
    {
        async static Task<int> Main()
        {
            var listener = UserNotificationListener.Current;

            if (! ApiInformation.IsTypePresent("Windows.UI.Notifications.Management.UserNotificationListener"))
            {
                Console.WriteLine("can't listen?");
            }
            
            while (true)
            {
                try
                {
                    Console.Write("Checking...");

                    // TODO: why does this fail "Element Not Found"?
                    IReadOnlyList<UserNotification> notifs = await listener.GetNotificationsAsync(NotificationKinds.Toast);

                    foreach (UserNotification notif in notifs)
                    {
                        Console.WriteLine("Found notification!");
                        Console.WriteLine(notif.Id);
                        // send HttpClient request of new notification
                    }

                    listener.ClearNotifications();

                    Console.WriteLine("done.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Waiting.");
                Thread.Sleep(300);
            }
        }
    }
}
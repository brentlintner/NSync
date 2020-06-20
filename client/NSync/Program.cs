using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Notifications.Management;
using System.Collections.Generic;
using Windows.UI.Notifications;
using Windows.Foundation.Metadata;

namespace NSync
{

    class NotificationInterceptor
    {
        private readonly UserNotificationListener listener;

        public NotificationInterceptor()
        {
            this.listener = UserNotificationListener.Current;
            //ToastNotificationManager.CreateToastNotifier("NSync");
        }

        public async void SyncNotifications()
        {
            IReadOnlyList<UserNotification> notifs = await this.listener.GetNotificationsAsync(NotificationKinds.Toast);

            foreach (UserNotification notif in notifs)
            {
                Console.WriteLine("Found notification!");
                Console.WriteLine(notif.Id);
                // send HttpClient request of new notification
            }

            this.listener.ClearNotifications();
        }
    }

    class Program
    {
        static void Main()
        {
            if (! ApiInformation.IsTypePresent("Windows.UI.Notifications.Management.UserNotificationListener"))
            {
                Console.WriteLine("can't listen?");
            }
            
            var interceptor = new NotificationInterceptor();

            while (true)
            {
                Console.Write("Checking...");
                try
                {
                    interceptor.SyncNotifications();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                Console.WriteLine("done.");
                Thread.Sleep(5000);
            }
        }
    }
}
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Data
{

    public interface IFcmService
    {
        Task SendMessage(NotificationModel message, string topic);
        Task SendMessagePrivate(NotificationModel message, string deviceId);
    }
    public class FcmService :IFcmService
    {
        private FirebaseApp fcm;
        private ApplicationDbContext _context;

        public FcmService(ApplicationDbContext context)
        {
            _context = context;
            try
            {
                fcm = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "key.json")),
                });
            }
            catch (Exception)
            {

            }
        }

        public Task SendMessage(NotificationModel notif, string topic)
        {
            var message = CreateMessage(notif);
            if (message != null)
            {
                message.Topic = topic;
                var messaging = FirebaseMessaging.DefaultInstance;
                messaging.SendAsync(message);
            }

            return Task.CompletedTask;
        }

        public Task SendMessagePrivate(NotificationModel notif, string deviceId)
        {
            var message = CreateMessage(notif);
            if (message != null)
            {
                message.Token = deviceId;
                var messaging = FirebaseMessaging.DefaultInstance;
                messaging.SendAsync(message);
            }
            return Task.CompletedTask;
        }

        private Message CreateMessage(NotificationModel notif)
        {
            notif.Id = 0;
            var list = new Dictionary<string, string>();
            list.Add("Created", DateTime.Now.Ticks.ToString());
            list.Add("Sender", notif.Sender);
            list.Add("NotificationType", notif.NotificationType.ToString());
            if (notif.Data != null)
            {
                foreach (var item in notif.Data)
                {
                    list.Add(item.Key, item.Value);
                }
            }

            var message = new Message()
            {
                Data = list,
                Notification = new Notification
                {
                    Title = notif.Title,
                    Body = notif.Body
                },
            };

            try
            {
                _context.Notifications.Add(notif);
                var result = _context.SaveChanges();
                if (result > 0)
                    return message;
            }
            catch(Exception ex)
            {
               
            }

            return null;
        }
    }
}

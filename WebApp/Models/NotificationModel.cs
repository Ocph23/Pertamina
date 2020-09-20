using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;

namespace WebApp.Models
{
    public class NotificationModel
    {
        public NotificationModel(string sender, string title, string body, NotificationType type, Dictionary<string,string> data=null)
        {
            Created = DateTime.Now;
            Sender = sender;
            NotificationType = type;
            Body = body;
            Data = data;
        }

        public NotificationModel() { }

        [Key]
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public NotificationType NotificationType { get; set; }

        [NotMapped]
        public Dictionary<string, string> Data {get;set;}
    }
}

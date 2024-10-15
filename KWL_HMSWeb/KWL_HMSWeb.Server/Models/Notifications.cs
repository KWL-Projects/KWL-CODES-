using System;

namespace KWL_HMSWeb.Server.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string DeviceToken { get; set; } // Store device token for push notifications
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

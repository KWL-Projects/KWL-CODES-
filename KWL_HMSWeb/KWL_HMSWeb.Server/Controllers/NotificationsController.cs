using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using KWL_HMSWeb.Services;

namespace KWL_HMSWeb.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly NotificationService _notificationService; // Inject NotificationService

        public NotificationsController(DatabaseContext context, NotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService; // Initialize NotificationService
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("Invalid notification data.");
            }

            // Add notification to the database
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Call the NotificationService to send the notification
            //await _notificationService.SendNotificationAsync(notification.DeviceToken, notification.Title, notification.Message);

            return Ok(notification);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }
    }
}

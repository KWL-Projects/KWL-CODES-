using KWL_HMSWeb.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace KWL_HMSWeb.Services
{
    public class NotificationService
    {
        private readonly DatabaseContext _context;

        public NotificationService(DatabaseContext context)
        {
            _context = context;
        }

        // Method to create a notification
        public async Task CreateNotification(Notifications notification)
        {
            _context.Notifications.Add(notification); // Assuming you've added the Notifications DbSet
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notifications>> GetAllNotifications()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notifications> GetNotificationById(int id)
        {
            return await _context.Notifications.FindAsync(id);
        }

    }
}

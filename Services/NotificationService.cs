
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Services
{
    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _context;

        public NotificationService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(string userId, int tributeId, int commentId)
        {
            var notification = new TributeNotification
            {
                UserId = userId,
                TributeId = tributeId,
                CommentId = commentId,
                IsRead = false
            };

            _context.TributeNotification.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TributeNotification>> GetUnreadNotificationsAsync(string userId)
        {
            return await _context.TributeNotification
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();
        }

        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _context.TributeNotification.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}

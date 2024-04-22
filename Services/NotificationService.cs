
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Services
//{
//    public class NotificationService : INotificationService
//    {
//        private readonly DatabaseContext _context;

//        public NotificationService(DatabaseContext context)
//        {
//            _context = context;
//        }

//        public async Task AddNotificationAsync(string userId, int tributeId, int commentId)
//        {
//            var notification = new TributeNotification
//            {
//                UserId = userId,
//                TributeId = tributeId,
//                CommentId = commentId,
//                IsRead = false
//            };

//            _context.TributeNotification.Add(notification);
//            await _context.SaveChangesAsync();
//        }

//        public async Task<IEnumerable<TributeNotification>> GetUnreadNotificationsAsync(string userId)
//        {
//            return await _context.TributeNotification
//                .Where(n => n.UserId == userId && !n.IsRead)
//                .ToListAsync();
//        }

//        public async Task MarkNotificationAsReadAsync(int notificationId)
//        {
//            var notification = await _context.TributeNotification.FindAsync(notificationId);
//            if (notification != null)
//            {
//                notification.IsRead = true;
//                await _context.SaveChangesAsync();
//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Services
{
    public class NotificationService : INotificationService
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(DatabaseContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddNotificationAsync(string userId, int tributeId, int commentId)
        {
            try
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
                _logger.LogInformation("Notification added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding notification.");
                throw;
            }
        }

        public async Task<IEnumerable<TributeNotification>> GetUnreadNotificationsAsync(string userId)
        {
            try
            {
                var notifications = await _context.TributeNotification
                    .Where(n => n.UserId == userId && !n.IsRead)
                    .ToListAsync();
                _logger.LogInformation($"Retrieved {notifications.Count} unread notifications for user {userId}.");
                return notifications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving unread notifications.");
                throw;
            }
        }

        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.TributeNotification.FindAsync(notificationId);
                if (notification != null)
                {
                    notification.IsRead = true;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Notification {notificationId} marked as read.");
                }
                else
                {
                    _logger.LogWarning($"Notification with ID {notificationId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while marking notification as read.");
                throw;
            }
        }
    }
}


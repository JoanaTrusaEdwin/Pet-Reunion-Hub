
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pet_Reunion_Hub.Services;
using PRHDATALIB.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PRHDATALIB.Models; // Assuming TributeNotification model is in a separate namespace
using Pet_Reunion_Hub.Services; // Assuming the notification service namespace

namespace Pet_Reunion_Hub.Pages
{
    public class NotificationsModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public NotificationsModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public List<TributeNotification> Notifications { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get unread notifications for the current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Assuming you're using ASP.NET Core Identity
            Notifications = (List<TributeNotification>)await _notificationService.GetUnreadNotificationsAsync(userId);

            return Page();
        }
    }
}

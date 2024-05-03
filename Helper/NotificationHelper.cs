using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using PRHDATALIB.Models;
using System.Security.Claims;

namespace Pet_Reunion_Hub.Helper
{
    public class NotificationHelper
    {
        public static int GetUnreadNotificationsCount(DatabaseContext context, UserManager<IdentityUser> userManager, ClaimsPrincipal user)
        {
            var userId = userManager.GetUserId(user); // Assuming User is accessible here
            return context.NEWNOTIFICATION.Count(n => n.UserId == userId && !n.IsRead);
        }
    }
}

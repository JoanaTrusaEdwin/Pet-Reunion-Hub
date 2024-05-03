using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pet_Reunion_Hub.Helper;

namespace Pet_Reunion_Hub.Controllers
{
    public class NotificationController : Controller
    {
      
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public NotificationController(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var unreadNotificationsCount = NotificationHelper.GetUnreadNotificationsCount(_context, _userManager, User);
            return View(unreadNotificationsCount);
        }
        [HttpGet]
        public IActionResult GetUnreadNotificationsCountAjax()
        {
            var unreadNotificationsCount = NotificationHelper.GetUnreadNotificationsCount(_context, _userManager, User);
            return Json(unreadNotificationsCount);
        }


        //}
        public IActionResult MarkAsRead(int id)
        {
            // Find the notification with the specified id
            var notification = _context.NEWNOTIFICATION.Find(id);
            if (notification != null)
            {
                // Mark the notification as read
                notification.IsRead = true;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }

}

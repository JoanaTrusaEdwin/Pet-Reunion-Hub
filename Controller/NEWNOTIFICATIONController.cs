using Microsoft.AspNetCore.Mvc;

namespace Pet_Reunion_Hub.Controllers
{
    public class NotificationController : Controller
    {
      
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        public NotificationController(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve all notifications from the database
            var notifications = _context.NEWNOTIFICATION.ToList();
            return View(notifications);
        }

        public int GetUnreadNotificationsCount()
        {
            return _context.NEWNOTIFICATION.Count(n => !n.IsRead);
        }
        //public IActionResult MarkAsRead(int id)
        //{
        //    // Find the notification with the specified id
        //    var notification = _context.NEWNOTIFICATION.Find(id);
        //    if (notification != null)
        //    {
        //        // Mark the notification as read
        //        notification.IsRead = true;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("MarkAsRead", "Notification", new { id = notification.Id });

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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pet_Reunion_Hub.Helper;

namespace Pet_Reunion_Hub.Controllers
{
    public class BaseController : Controller
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BaseController(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var unreadCount = NotificationHelper.GetUnreadNotificationsCount(_context, _userManager, User);
            ViewData["UnreadNotificationsCount"] = unreadCount;
            base.OnActionExecuting(context);
        }
    }
}


//using Microsoft.AspNetCore.Mvc.RazorPages;
//using PRHDATALIB.Models;
//using System.Collections.Generic;
//using System.Linq;
//using PRHDATALIB.Models;// Import your Notification model here

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
//{
//    public class TributeNotificationsModel : PageModel
//    {
//        private readonly DatabaseContext _context; // Inject your DatabaseContext here

//        public TributeNotificationsModel(DatabaseContext context)
//        {
//            _context = context;
//        }

//        public List<TributeNotification> Notifications { get; set; }

//        public void OnGet()
//        {
//            // Fetch notifications from the database
//            Notifications = _context.TributeNotification.ToList();
//        }
//    }
//}

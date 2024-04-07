using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.CONTAINERS
{
    [Authorize]
    public class ContainersModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ContainersModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<CONTAINER> Containers { get; set; }

        public void OnGet()
        {
            Containers = _context.CONTAINER
                .Include(c => c.User)
                .ToList();


            if (Containers == null || Containers.Count == 0)
            {
                // Handle the case where there are no containers
                ViewData["Message"] = "You have not created any containers yet.";
                return;
            }
        }
    }
}


﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.TEST.TESTBITS
//{
//    public class IndexModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public IndexModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        public IList<TESTBIT> TESTBIT { get;set; } = default!;

//        public async Task OnGetAsync()
//        {
//            if (_context.TESTBIT_1 != null)
//            {
//                TESTBIT = await _context.TESTBIT_1
//                .Include(t => t.User).ToListAsync();
//            }
//        }
//    }
//}

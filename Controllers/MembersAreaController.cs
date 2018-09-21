using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CCT
{
    public class MembersAreaController : Controller
    {
        protected AppDBContext mContext;

        public MembersAreaController(AppDBContext context) 
        {
            mContext = context;
        }


        // Private Area
        [Authorize]
        public IActionResult Index()
        {
            mContext.Database.EnsureCreated();
            return View();
        }
    }
}

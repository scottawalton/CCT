using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CCT.Admin
{
    // Makes entire area private to only Admins

    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;

        #endregion

        #region Default Constructor

        public HomeController(AppDBContext _context) 
        {
            mContext = _context;
        }

        #endregion

        public IActionResult Index()
        {
            mContext.Database.EnsureCreated();
            return View();
        }

        public IActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            List<Event> eventList = new List<Event>();

            // Get events from database 

            return Json(eventList);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCT.Models;

namespace CCT
{
    public class HomeController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;
        #endregion

        #region Default Const
        public HomeController(AppDBContext context) 
        {
            mContext = context;
        }
        #endregion


        public IActionResult Index()
        {
            mContext.Database.EnsureCreated();

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

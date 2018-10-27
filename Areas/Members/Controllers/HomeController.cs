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

namespace CCT.Members
{
    [Area("Members")]
    [Authorize(Roles="member,admin")]
    public class HomeController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;

        #endregion

        #region Default Constructor

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
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
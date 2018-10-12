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
    public class UsersController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;
        protected UserManager<User> userManager;

        public class UserList
        {
            public List<User> userlist {get; set;}
        }

        #endregion

        #region Default Constructor

        public UsersController(AppDBContext _context,
                                UserManager<User> _userManager) 
        {
            userManager = _userManager;
            mContext = _context;
        }

        #endregion
        public IActionResult Index()
        {

            UserList users = new UserList();

            users.userlist = mContext.Users.ToList();

            return View(users);
        }
    }
}

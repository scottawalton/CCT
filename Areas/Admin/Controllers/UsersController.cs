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
using System.ComponentModel.DataAnnotations;

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


        #region Delete User

        public async Task<IActionResult> DeleteUser(string id)
        {

            var userSearch = mContext.Users.FindAsync(id);
            User user = userSearch.Result;
            await userManager.DeleteAsync(user);

            return RedirectToAction("Index", "Users");
        }

        #endregion

        #region View User
        public IActionResult ViewUser(string id)
        {
            var userSearch = mContext.Users.FindAsync(id);
            User user = userSearch.Result;


            return View(user);

        }
        #endregion

        #region Edit User
        public IActionResult EditUser(string id)
        {
            var userSearch = mContext.Users.FindAsync(id);

            User user = userSearch.Result;

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(User revisedUser)
        {

            var userSearch = mContext.Users.FindAsync(revisedUser.Id);
            User user = userSearch.Result;


            if (!user.Equals(null))
            {
                user.Email = revisedUser.Email;
                user.FirstName = revisedUser.FirstName;
                user.LastName = revisedUser.LastName;

                await mContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Users");
        }
        #endregion

        #region Create User

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(PortalController.userClaim User)
        {

            var result = PortalController.CreateUser(User);

            if (result.Result.Succeeded) {

                return RedirectToAction("Index", "Users");
            }  
            else {
                ViewBag.errors = result.Result.Errors;
                return View();
            }
        }
        #endregion
    }
}

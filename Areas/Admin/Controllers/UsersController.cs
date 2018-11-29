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
        protected static UserManager<User> userManager;
        protected static RoleManager<IdentityRole> roleManager;

        public class UserList
        {
            public List<User> userlist {get; set;}
        }
        public class userClaim : User
        {
            [Required]
            [DataType(DataType.Password)]
            public string password {get; set;}

            public bool admin {get; set;}
        }

        #endregion

        #region Default Constructor

        public UsersController(AppDBContext _context,
                                UserManager<User> _userManager,
                                RoleManager<IdentityRole> _roleManager) 
        {
            roleManager = _roleManager;
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
                user.ApartmentNumber = revisedUser.ApartmentNumber;

                await mContext.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Users");
        }
        #endregion

        #region Create User
        /// <summary>
        /// Attempts to create a new user and write it to the database using Usermanager
        /// </summary>
        /// <returns>Returns an IdentityResult</returns>
        public static async Task<IdentityResult> NewUser(userClaim User)
        {

            User.UserName = User.ApartmentNumber.ToString();

            // WIP -- Hashing pass with 265 SHA
            IdentityResult result = await userManager.CreateAsync(User, User.password);

            // TODO
            // Also: remove RoleManager in constructor and protected members
            // as well as the checkbox on the view

            if (User.admin)
            {
                IdentityRole admin = new IdentityRole("admin");
                await roleManager.CreateAsync(admin);
                await userManager.AddToRoleAsync(User, "admin");
            }
            else 
            {
                IdentityRole member = new IdentityRole("member");
                await roleManager.CreateAsync(member);
                await userManager.AddToRoleAsync(User, "member");
            }

            return result;

        }

        /// <summary>
        /// Returns a View where the user is able to fill out a form to sign up for an account
        /// </summary>
        public IActionResult CreateUser()
        {
            return View();
        }

        /// <summary>
        /// Attempts to create a new user and lets us know what happens.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser(userClaim User)
        {

            if (ModelState.IsValid && User.FirstName!=null) {
                var result = await NewUser(User);

                    // Redirects them to the Members area if successful; otherwise, reloads page and shows error
                if (result.Succeeded) {

                    return RedirectToAction("Index", "Users");
                }  
                else {
                    ViewBag.errors = result.Errors;
                    return View();
                }
            }
            ViewBag.errors = "Please ensure all fields are correctly filled.";
            return View();
        }
        #endregion
    }
}

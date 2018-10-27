using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CCT
{
    public class PortalController : Controller
    {

        #region Protected Members
        protected static UserManager<User> userManager;
        protected static SignInManager<User> signInManager;
        protected static RoleManager<IdentityRole> roleManager;
        protected static AppDBContext context;
        public class userClaim : User
        {
            [Required]
            [DataType(DataType.Password)]
            public string password {get; set;}

            public bool admin {get; set;}
        }

        public class LogInModel
        {
            public string email {get; set;}
            public string password {get; set;}
        }

        #endregion

        #region Default Contstructor
        public PortalController(AppDBContext _context,
                                UserManager<User> _userManager,  
                                SignInManager<User> _signInManager,
                                RoleManager<IdentityRole> _roleManager) 
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;

        }
        #endregion

        #region Sign In

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LogInModel credentials)
        {

            var result = signInManager.PasswordSignInAsync(credentials.email, credentials.password, true, false);

            if (ModelState.IsValid)
            {
                if (result.Result.Succeeded) 
                {
                    return RedirectToAction("Index", "Members");
                }
                else
                {
                    ViewBag.errors = "Log in attempt failed. Incorrect username or password.";
                    return View();
                }
            }

            return View();

        }

        #endregion

        #region Sign Out
        /// <summary>
        /// Signs out user using Identity SigninManager
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region New User
        /// <summary>
        /// Attempts to create a new user and write it to the database using Usermanager
        /// </summary>
        /// <returns>Returns an IdentityResult</returns>
        public static async Task<IdentityResult> CreateUser(userClaim User)
        {
            User.UserName = User.FirstName + "_" + User.LastName;

            // WIP -- Hashing pass with 265 SHA

            IdentityResult result = await userManager.CreateAsync(User, User.password);


            ///////////////// REMOVE AFTER DEVELOPMENT COMPLETE ///////////////////////
            // Also: remove RoleManager in constructor and protected members
            // as well as the checkbox on the view

            if (User.admin)
            {
                IdentityRole admin = new IdentityRole("admin");
                await roleManager.CreateAsync(admin);
                await userManager.AddToRoleAsync(User, "admin");
            }

            return result;
        }

        /// <summary>
        /// Returns a View where the user is able to fill out a form to sign up for an account
        /// </summary>
        public IActionResult NewUser()
        {
            return View();
        }

        /// <summary>
        /// Attempts to create a new user and lets us know what happens.
        /// </summary>
        [HttpPost]
        public IActionResult NewUser(userClaim User)
        {

            var result = CreateUser(User);

            // Redirects them to the Members area if successful; otherwise, reloads page and shows error
            if (result.Result.Succeeded) {

                return RedirectToAction("Index", "Members");
            }  
            else {
                ViewBag.errors = result.Result.Errors;
                return View();
            }
        }
        #endregion

    }
}
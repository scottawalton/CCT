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
        protected static AppDBContext context;

        public class LogInModel
        {
            public string email {get; set;}
            public string password {get; set;}
        }

        #endregion

        #region Default Contstructor
        public PortalController(AppDBContext _context,
                                UserManager<User> _userManager,  
                                SignInManager<User> _signInManager) 
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;

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

            // I may instead opt for global validation that none of the required parameters are null
            // For now, this is sufficient. We only have 2 instances where it is required.
            if (credentials.email != null && ModelState.IsValid)
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

    }
}
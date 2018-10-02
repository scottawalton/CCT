using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CCT.Controllers
{
    public class PortalController : Controller
    {

        #region Protected Members
        protected static UserManager<User> userManager;
        protected static SignInManager<User> signInManager;
        protected static AppDBContext context;
        public class userClaim : User
        {
            [Required]
            [DataType(DataType.Password)]
            public string password {get; set;}
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
                                SignInManager<User> _signInManager ) 
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;

        }
        #endregion

        #region Sign In
        public IActionResult Index()
        {
            
            // accept credentials

            // verify

            // create cookie

            // redirect

            return View();
        }
        #endregion

        #region New User
        /// <summary>
        /// Attempts to create a new user and write it to the database using Usermanager
        /// </summary>
        /// <returns>Returns an IdentityResult</returns>
        public async Task<IdentityResult> CreateUser(userClaim User)
        {
            User.UserName = User.Email;

            // WIP -- Hashing pass with 265 SHA

            IdentityResult result = await userManager.CreateAsync(User, User.password);

            return result;
        }

        /// <summary>
        /// Returns a View where the user is able to fill out a form to sign up for an account
        /// </summary>
        public IActionResult NewUser()
        {

            var user = new userClaim();
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
                signInManager.SignInAsync(User, true);
                return View("../MembersArea/Index", User); 
            }  
            else {
                ViewBag.errors = result.Result.Errors;
                return View();
            }
        }
        #endregion
    }
}
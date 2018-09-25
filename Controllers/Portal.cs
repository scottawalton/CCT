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

        public IActionResult Index()
        {
            
            // accept credentials

            // verify

            // create cookie

            // redirect

            return View();
        }

        public IActionResult NewUser()
        {

            var user = new userClaim();

            return View();
        }

        [HttpPost]
        public IActionResult UserCreationAttempted(userClaim User)
        {
            
            var result = CreateUser(User);

            return View(User);
        }

        public async Task<IdentityResult> CreateUser(userClaim User)
        {
            byte[] passInBytes = Encoding.UTF8.GetBytes(User.password);
            SHA256Managed crypt = new SHA256Managed();
            byte[] crypto = crypt.ComputeHash(passInBytes);

            // WIP -- Hashing pass with 265 SHA
            var result = await userManager.CreateAsync(User, User.password);

            return result;
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            var user = new User();

            return View();
        }

        [HttpPost]
        public IActionResult UserCreationAttempted(User User)
        {
            
            var result = CreateUser(User);

            return View(User);
        }

        public async Task<IdentityResult> CreateUser(User User)
        {
            var result = await userManager.CreateAsync(User, User.password);

            return result;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace CCT.Controllers
{
    public class PortalController : Controller
    {
        // 
        // GET: /Portal/

        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /Portal/Welcome/ 

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            // Redirect them to Members Index
            return View();
        }
    }
}
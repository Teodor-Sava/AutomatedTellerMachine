using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedTellerMachine.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AutomatedTellerMachine.Controllers
{
    public class HomeController : Controller
    {   private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccountId = db.Checking.Where(c => c.AplicationUserId == userId).First().Id;
            ViewBag.CheckingAccountId = checkingAccountId;
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = manager.FindById(userId);
            
            return View();
        }
        public ActionResult Serial(string letterCase)
        {
            var serial = "ASPNETMVC5ATM1";
            if (letterCase == "lower")
            {
                return Content(serial.ToLower());
            }
            return Content(serial);
        }
    }
}
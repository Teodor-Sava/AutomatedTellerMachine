using System.Linq;
using System.Web.Mvc;
using AutomatedTellerMachine.Repositories;
using Microsoft.AspNet.Identity;

namespace AutomatedTellerMachine.Controllers
{
    [Authorize]
    public class CheckingAccountController : Controller
    {
        private readonly IRepository repo;
        // GET: CheckingAccount

        public CheckingAccountController(IRepository repository)
        {
            repo = repository;
        }

  

        // GET: CheckingAccount/Details/5
        public ActionResult Details()
        {
            var userId = User.Identity.GetUserId();
            var checkingAccount = repo.FindByAccountId(userId);

            return View(checkingAccount);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DetailsForAdmin(int id)
        {
            var checkingAccount =repo.FindById(id);
            return View("Details", checkingAccount);
        }

        [HttpGet]
        public ActionResult Statement(int checkingAccountId)
        {
            var checkingAccount =repo.FindById(checkingAccountId);
            return View(checkingAccount.Transactions.ToList());
        }
    }
}
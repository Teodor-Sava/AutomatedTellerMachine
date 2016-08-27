using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomatedTellerMachine.Models;
using AutomatedTellerMachine.Services;

namespace AutomatedTellerMachine.Controllers
{
    [Authorize]
    public class TransactionController : Controller

    {
        private IApplicationDbContext db;

        public TransactionController()
        {
            db = new ApplicationDbContext();
        }

        public TransactionController(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        // GET: Transaction/Deposit
        public ActionResult Deposit(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                var service = new CheckingAccountService(db);
                service.UpdateBalance(transaction.CheckingAccountId);
                return RedirectToAction("Index", "Home");
            }
            else return View();
        }
        //Get: Transaction/Withdrawl
        public ActionResult Withdrawl(int checkingAcoountId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Withdrawl(Transaction transaction)
        {
            var checkingAccount = db.Checking.Find(transaction.CheckingAccountId);
            if (checkingAccount.Balance < transaction.Amount)
            {
                ModelState.AddModelError("Amount", "You have insuficient funds");
            }
            if (ModelState.IsValid)
            {
                transaction.Amount = -transaction.Amount;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                var service = new CheckingAccountService(db);
                service.UpdateBalance(transaction.CheckingAccountId);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Transfer(int checkingAccountId)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Transfer(TransferViewModel transfer)
        {
            var sourceCheckingAccount = db.Checking.Find(transfer.CheckingAccoutId);
            if (sourceCheckingAccount.Balance < transfer.Amount)
            {
                ModelState.AddModelError("Amount","You have insufficient funds !");
            }
            var destinationCheckingAccount =
                db.Checking.Where(c => c.AccountNumber == transfer.DestinationCheckingAccountNumber).FirstOrDefault();
            if (destinationCheckingAccount == null)
            {
                ModelState.AddModelError("DetinationCheckingAccountNumber","Invalid account number");

            }
            if (ModelState.IsValid)
            {
                db.Transactions.Add(new Transaction
                {
                    CheckingAccountId = transfer.CheckingAccoutId,
                    Amount = -transfer.Amount
                });
                db.Transactions.Add(new Transaction
                {
                    CheckingAccountId = destinationCheckingAccount.Id,
                    Amount = transfer.Amount
                });
                db.SaveChanges();
                var service = new CheckingAccountService(db);
                service.UpdateBalance(transfer.CheckingAccoutId);
                service.UpdateBalance(destinationCheckingAccount.Id);
                return PartialView("_TransferSuccess", transfer); 
            }
            return PartialView("_TranserForm");
        }
    }
}
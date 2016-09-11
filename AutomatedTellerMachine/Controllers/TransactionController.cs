using System;
using System.Web.Mvc;
using AutomatedTellerMachine.Models;
using AutomatedTellerMachine.Repositories;

namespace AutomatedTellerMachine.Controllers
{
    [Authorize]
    public class TransactionController : Controller

    {
        private readonly IRepository _repo;

  
        public TransactionController(IRepository repo)
        {
            _repo = repo;
        }


        // GET: Transaction/Deposit
        public ActionResult Deposit(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            transaction.Message = "Deposit";
            if (ModelState.IsValid)
            {
                if (_repo.DepositAmount(transaction))
                {
                    return RedirectToAction("Index", "Home");
                }
                var error = new TransactionError
                {
                    Title = "Transaction Denied",
                    Message = "Could not deposit the amount"
                };
                return View("TransactionError", error);

            }
            return View();
        }


        public ActionResult QuickCash(int checkingAccountId, decimal amount)
        {
            try
            {
                var balance = _repo.GetBalace(checkingAccountId);
                if (balance < amount)
                {
                    var error = new TransactionError
                    {
                        Title = "Transaction Denied",
                        Message = "You don't have the required amount in your account"
                    };
                    return View("TransactionError", error);
                }

                if (_repo.AddTransaction(checkingAccountId, -amount, "Quick cash"))
                {
                    return RedirectToAction("Statement", "CheckingAccount", new { checkingAccountId = checkingAccountId });
                }
                var error2 = new TransactionError
                {
                    Title = "Transaction Denied",
                    Message = "Could not deposit the amount"
                };
                return View("TransactionError", error2);
            }
            catch (Exception e)
            {
                var error2 = new TransactionError
                {
                    Title = "Transaction Denied",
                    Message = e.Message
                };
                return View("TransactionError", error2);
            }
          
        }

        //Get: Transaction/Withdrawl
        public ActionResult Withdrawl(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Withdrawl(Transaction transaction)
        {
            try
            {
                var balance = _repo.GetBalace(transaction.CheckingAccountId);
                if (balance < transaction.Amount)
                {
                    ModelState.AddModelError("Amount", "You have insuficient funds");
                }
                if (ModelState.IsValid)
                {
                    if (_repo.AddTransaction(transaction.CheckingAccountId, -transaction.Amount, "Withdrawal"))
                    {
                        return RedirectToAction("Statement", "CheckingAccount", new { checkingAccountId = transaction.CheckingAccountId });
                    }
                    var error = new TransactionError
                    {
                        Title = "Transaction Denied",
                        Message = "Could not withdraw"
                    };
                    return View("TransactionError", error);

                }
                ModelState.AddModelError("InvalidAccount", "Invalid account.");
                return View();
            }
            catch (Exception e)
            {
                var error2 = new TransactionError
                {
                    Title = "Transaction Denied",
                    Message = e.Message
                };
                return View("TransactionError", error2);
            }
        }

        public ActionResult Transfer(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(TransferViewModel transfer)
        {
            try
            {
                var balance = _repo.GetBalace(transfer.CheckingAccountId);
                if (balance < transfer.Amount)
                {
                    ModelState.AddModelError("Amount", "You have insufficient funds!");
                }


                // check for a valid destination account

                if (!_repo.AccountExists(transfer.DestinationCheckingAccountId))
                {
                    ModelState.AddModelError("DestinationCheckingAccountId", "Invalid destination account number.");
                }

                // add debit/credit transactions and update account balances
                if (ModelState.IsValid)
                {
                    if (_repo.Transfer(transfer.CheckingAccountId, transfer.DestinationCheckingAccountId, transfer.Amount,
                            transfer.Message))
                    {
                        return PartialView("_TransferSuccess", transfer);
                    }
                    var error = new TransactionError
                    {
                        Title = "Transaction Denied",
                        Message = "Could not transfer successfuly"
                    };
                    return View("TransactionError", error);

                }
                return PartialView("_TransferForm");
            }
            catch (Exception e)
            {

                var error2 = new TransactionError
                {
                    Title = "Transaction Denied",
                    Message = e.Message
                };
                return View("TransactionError", error2); 
            }
        }
    }
}
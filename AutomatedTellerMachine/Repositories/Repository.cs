using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using AutomatedTellerMachine.Models;

namespace AutomatedTellerMachine.Repositories
{
    public class Repository:IRepository
    {
        private readonly IApplicationDbContext db;
        public Repository(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public bool DepositAmount(Transaction transaction)
        {
            try
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                UpdateBalance(transaction.CheckingAccountId);
                return true;
            }
            catch (Exception)
            {
                return false;
              
            }
        }

        public bool AddTransaction(int checkingAccountId, decimal amount,string message=null)
        {
            try
            {
                db.Transactions.Add(new Transaction { CheckingAccountId = checkingAccountId, Amount = amount, Message = message });
                db.SaveChanges();
                UpdateBalance(checkingAccountId);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public decimal GetBalace(int checkingAccountId)
        {
            var sourceCheckingAccount = db.Checking.Find(checkingAccountId);
            if (sourceCheckingAccount == null)
            {
                throw new Exception("Unknown Account");
            }
            return sourceCheckingAccount.Balance;
        }

        private void UpdateBalance(int checkingAccountId)
        {
            var checkingAccount = db.Checking.First(c => c.Id == checkingAccountId);
            checkingAccount.Balance =
                db.Transactions.Where(c => c.CheckingAccountId == checkingAccountId).Sum(c => c.Amount);
            db.SaveChanges();
        }

        public bool CreateCheckingAccount(string firstName, string lastName, string userId, decimal initialBalance)
        {
            var random = new Random();
            var random2 = new Random();
            int accountNumber = random.Next(10000) + random2.Next(10000, 10000000);

            var checkingAccount = new CheckingAccount
            {
                FirstName = firstName,
                LastName = lastName,
                Balance = initialBalance,
                AplicationUserId = userId,
                AccountNumber = accountNumber
            };
            db.Checking.Add(checkingAccount);
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                return false;
            }
        }

        public bool AccountExists(int destinationCheckingAccount)
        {
            var account =
              db.Checking.FirstOrDefault(c => c.Id == destinationCheckingAccount);
            return account!=null;
        }

        public bool Transfer(int sourceAccount, int destinationAccount, decimal amount, string message = null)
        {
            try
            {
                AddTransaction(sourceAccount, -amount);
                AddTransaction(destinationAccount, amount);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public CheckingAccount FindById(int id)
        {
            return db.Checking.Find(id);
        }

        public CheckingAccount FindByAccountId(string userId)
        {
         return   db.Checking.First(c => c.AplicationUserId == userId);
        }
    }
    
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutomatedTellerMachine.Models;
using Microsoft.Ajax.Utilities;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace AutomatedTellerMachine.Services
{
    public class CheckingAccountService
    {
        private IApplicationDbContext db;

        public CheckingAccountService(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        public void CreateCheckingAccount(string firstName, string lastName, string userId, decimal initialBalance)
        {   
            Random random = new Random();
            Random random2 = new Random();
            long accountNumber = random.Next(10000)+random2.Next(10000,1000000);
            
            var checkingAccount = new CheckingAccount
            {   
                FirstName = firstName,
                LastName = lastName,
                Balance = initialBalance,
                AplicationUserId = userId,
                AccountNumber = accountNumber
            };
            db.Checking.Add(checkingAccount);
            try { db.SaveChanges();}
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
            }
        }

        public void UpdateBalance(int checkingAccountId)
        {
            var checkingAccount = db.Checking.Where(c => c.Id == checkingAccountId).First();
            checkingAccount.Balance = db.Transactions.Where(c => c.CheckingAccountId == checkingAccountId).Sum(c => c.Amount);
            db.SaveChanges();
        }
    }
}
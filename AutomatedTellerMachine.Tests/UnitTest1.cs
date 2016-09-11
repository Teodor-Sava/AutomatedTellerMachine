using System;
using System.Web.Mvc;
using System.Web.Util;
using AutomatedTellerMachine.Controllers;
using AutomatedTellerMachine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomatedTellerMachine.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void BalanceIsCorrectAfterDeposit()
        {
            var fakeDb = new FakeApplicationDbContext();
            fakeDb.Checking = new FakeDbSet<CheckingAccount>();

            var checkingAccount = new CheckingAccount {Id = 1, AccountNumber = 121345, Balance = 0};
            fakeDb.Checking.Add(checkingAccount);
            fakeDb.Transactions = new FakeDbSet<Transaction>();
            var transactionController = new TransactionController(fakeDb);
            transactionController.DepositAmount(new Transaction{ Amount = 25, CheckingAccountId = 1});
            
            Assert.AreEqual(25,checkingAccount.Balance);
        }

        [TestMethod]
        public void TransferIsCorrectBetweenAccounts() 
        {
            var fakeDb = new FakeApplicationDbContext();
            fakeDb.Checking = new FakeDbSet<CheckingAccount>();

            var checkingAccount = new CheckingAccount { Id = 3, AccountNumber = 2342, Balance = 0 };

            fakeDb.Checking.Add(checkingAccount);
            var destinationAccount = new CheckingAccount { Id = 4, AccountNumber = 24312, Balance = 0 };
            fakeDb.Checking.Add(destinationAccount);

            fakeDb.Transactions = new FakeDbSet<Transaction>();
            var transactionController = new TransactionController(fakeDb);

            transactionController.Transfer(new TransferViewModel
            {
                CheckingAccountId = checkingAccount.Id,
                Amount = 30,
                DestinationCheckingAccountNumber = destinationAccount.Id
            });


            Assert.AreEqual(30, destinationAccount.Balance);
            Assert.AreEqual(10, checkingAccount.Balance);
        }
    }
}

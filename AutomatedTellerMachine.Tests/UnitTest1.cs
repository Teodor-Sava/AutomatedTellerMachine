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
        public void FooActionReturnAboutView()
        {
            var homeController = new HomeController();
            var result = homeController.Foo() as ViewResult;
            Assert.AreEqual("About", result.ViewName);
        }

        [TestMethod]
        public void ContactFormSaysThanks()
        {
            var homeController = new HomeController();
            var result = homeController.Contact("yo whatsapp");
            Assert.IsNotNull(result.ToString());
        }

        [TestMethod]
        public void BalanceIsCorrectAfterDeposit()
        {
            var fakeDb = new FakeApplicationDbContext();
            fakeDb.Checking = new FakeDbSet<CheckingAccount>();

            var checkingAccount = new CheckingAccount {Id = 1, AccountNumber = "000123TEST", Balance = 0};
            fakeDb.Checking.Add(checkingAccount);
            fakeDb.Transactions = new FakeDbSet<Transaction>();
            var transactionController = new TransactionController(fakeDb);
            transactionController.Deposit(new Transaction{ Amount = 25, CheckingAccountId = 1});
            
            Assert.AreEqual(25,checkingAccount.Balance);
        }
    }
}

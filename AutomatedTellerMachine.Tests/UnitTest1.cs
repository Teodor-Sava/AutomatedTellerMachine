using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Util;
using AutomatedTellerMachine.Controllers;
using AutomatedTellerMachine.Models;
using AutomatedTellerMachine.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

            var checkingAccount = new CheckingAccount { Id = 1, AccountNumber = 121345, Balance = 0 };
            fakeDb.Checking.Add(checkingAccount);
            fakeDb.Transactions = new FakeDbSet<Transaction>();
            var repository = new Repository(fakeDb);
            repository.DepositAmount(new Transaction { Amount = 25, CheckingAccountId = 1 });

            Assert.AreEqual(25, checkingAccount.Balance);
        }

        [TestMethod]
        public void TransferIsCorrectBetweenAccounts()
        {
            var fakeDb = new FakeApplicationDbContext
            {
                Checking = new FakeDbSet<CheckingAccount>(),
                Transactions = new FakeDbSet<Transaction>()
            };
            var repository = new Repository(fakeDb);

            var checkingAccount = new CheckingAccount { Id = 3, AccountNumber = 2342, Balance = 0 };

            fakeDb.Checking.Add(checkingAccount);
            repository.DepositAmount(new Transaction() {Amount = 40, CheckingAccount = checkingAccount,CheckingAccountId = checkingAccount.Id});
            var destinationAccount = new CheckingAccount { Id = 4, AccountNumber = 24312, Balance = 0 };
            fakeDb.Checking.Add(destinationAccount);

            repository.Transfer( checkingAccount.Id,
               destinationAccount.Id,
               30
            );

            Assert.AreEqual(30, destinationAccount.Balance);
            Assert.AreEqual(10, checkingAccount.Balance);
        }

        [TestMethod]
        public void MoqTest()
        {
            var mDbContext = new Mock<IApplicationDbContext>();
            //db transaction should fail because we have two fileds that are required and we have not initialized them
            mDbContext.Setup(t => t.Transactions.Add(new Transaction())).Throws(new Exception());
           
             Repository repo = new Repository(mDbContext.Object);
            Assert.IsFalse(repo.DepositAmount(new Transaction()));        
        }
    }
}

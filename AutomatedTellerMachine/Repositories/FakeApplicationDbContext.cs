using System.Data.Entity;
using AutomatedTellerMachine.Models;

namespace AutomatedTellerMachine.Repositories
{
    public class  FakeApplicationDbContext : IApplicationDbContext
    {
        public IDbSet<CheckingAccount> Checking { get; set; }
        public IDbSet<Transaction> Transactions { get; set; }

        public int SaveChanges()
        {
            return 0;
        }
    }
}
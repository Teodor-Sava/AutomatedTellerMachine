using System.Data.Entity;
using AutomatedTellerMachine.Migrations;
using AutomatedTellerMachine.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutomatedTellerMachine.Repositories
{
    public interface IApplicationDbContext
    {
        IDbSet<CheckingAccount> Checking { get; set; }
        IDbSet<Transaction> Transactions { get; set; }

        int SaveChanges();
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,IApplicationDbContext
        {   
            
            public ApplicationDbContext()
                : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,Configuration>());
            base.OnModelCreating(modelBuilder); 
        }
            public IDbSet<CheckingAccount> Checking { get; set; }

            public IDbSet<Transaction> Transactions { get; set; }

          }
}
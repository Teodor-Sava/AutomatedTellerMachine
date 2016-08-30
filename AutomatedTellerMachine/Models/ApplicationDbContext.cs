using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AutomatedTellerMachine.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AutomatedTellerMachine.Models
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

            public static Models.ApplicationDbContext Create()
            {
                return new Models.ApplicationDbContext();
            }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext,Configuration>());
            base.OnModelCreating(modelBuilder); 
        }
            public IDbSet<CheckingAccount> Checking { get; set; }

            public IDbSet<Transaction> Transactions { get; set; }
            
    }

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
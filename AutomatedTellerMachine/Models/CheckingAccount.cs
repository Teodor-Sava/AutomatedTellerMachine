using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace AutomatedTellerMachine.Models
{
    public class CheckingAccount
    {  
        public int Id { get; set; }
       
        [Display(Name = "Account #")]
        public long AccountNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Name
        {
            get { return String.Format("{0} {1}", this.FirstName, this.LastName); }
        }
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public virtual ApplicationUser User { get; set; }
      

        [Required]
        public string AplicationUserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
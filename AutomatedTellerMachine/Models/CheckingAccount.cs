using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedTellerMachine.Models
{
    public class CheckingAccount
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Account #")]
        public long AccountNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Name
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public virtual ApplicationUser User { get; set; }


        [Required]
        public string AplicationUserId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
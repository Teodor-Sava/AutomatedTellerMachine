using System.ComponentModel.DataAnnotations;

namespace AutomatedTellerMachine.Models
{
    public class TransferViewModel
    {
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public int CheckingAccountId { get; set; }

        [Required]
        [Display(Name = "To Account #")]
        public int DestinationCheckingAccountId { get; set; }

        public string Message { get; set; }

    }
}
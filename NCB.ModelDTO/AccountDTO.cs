using NCB.Models;
using System.ComponentModel.DataAnnotations;

namespace NCB.ModelDTO
{
    public class AccountDTO: CreateAccountDTO
    {
        public int Id { get; set; }
    }

    public class CreateAccountDTO
    {
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "Account Number must be 7 digits")]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "Currency Id")]
        public int? CurrencyId { get; set; }


        [Display(Name = "Card Number")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Card Number must be 12 digits")]
        public string? CardNumber { get; set; }

        [Display(Name = "Card Expiration Date")]
        public string? CardExpirationDate { get; set; }

        [Display(Name = "Card Security Code")]
        public string? CardSecurityCode { get; set; }

        [Required]
        [Display(Name = "Account Tyoe")]
        public AccountType AccountType { get; set; }

        [DataType(DataType.Currency)]
        public double Balance { get; set; }

        public DateTime CreateDate { get; set; }

        public Currency Currency { get; set; }
        public UserDTO? User { get; set; }
    }
}
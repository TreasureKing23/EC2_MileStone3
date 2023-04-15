using NCB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NCB.ModelDTO
{
    public class TransactionDTO : CreateTransactionDTO

    {
        public int Id { get; set; }
    }


    public class CreateTransactionDTO
    {
        public String? UserId { get; set; }
        public String? Source { get; set; }
        public string? AccountNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TransactionType TransactionType { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        public string? CardNumber { get; set; }

        [Required]
        public string? CardExpirationDate { get; set; }

        [Required]
        public string? CardSecurityCode { get; set; }

        [Display(Name ="Customer")]
        public UserDTO? User { get; set; }

    }



}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.Models
{
    public class Order
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public string ShoeId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? ShippingAddress { get; set; }

        public string CardNumber { get; set;}

        [NotMapped]
        public string? CardExpirationDate { get; set;}

        [NotMapped]
        public string? CardSecurityCode { get; set;}

        public int  Quantity { get; set;}

        public double Total { get; set;}
    }
}

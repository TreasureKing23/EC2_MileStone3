using _1903966_Milestone2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1903966_Milestone2.ViewModels
{
    public class OrderViewModel : CreateOrderViewModel
    {
        public int Id { get; set; }

        public OrderViewModel() { }

        public OrderViewModel(Order model)
        {
            UserId = model.UserId.ToString();
            ShoeId = model.ShoeId;
            TransactionDate = model.TransactionDate;
            ShippingAddress = model.ShippingAddress;
            CardNumber = model.CardNumber;
            CardExpirationDate = model.CardExpirationDate;
            CardSecurityCode = model.CardSecurityCode;
            Quantity = model.Quantity;
            Total = model.Total;

        }
    }

    public class CreateOrderViewModel
    {

        public string? UserId { get; set; }

        public string? ShoeId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }

        public string? ShippingAddress { get; set; }

        public string? CardNumber { get; set; }

       
        public string? CardExpirationDate { get; set; }

       
        public string? CardSecurityCode { get; set; }

        [Range(1, 5)]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public double Total { get; set; }

        public ShoeViewModel? Shoe { get; set; }

        public Order ConvertViewModelToModel(OrderViewModel model)
        {
            return new Order
            {
                UserId = new Guid(model.UserId),
                ShoeId = model.ShoeId,
                TransactionDate = model.TransactionDate,
                ShippingAddress = model.ShippingAddress,
                CardNumber = model.CardNumber,
                CardExpirationDate = model.CardExpirationDate,
                CardSecurityCode = model.CardSecurityCode,
                Quantity = model.Quantity,
                Total = model.Total
            };
        }

        public List<OrderViewModel> ConvertModelToViewModelList(List<Order> modelList)
        {
            return modelList.Select(x => new OrderViewModel(x)).ToList();
        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowerShop.Core.Entities;

namespace FlowerShop.Web.Models
{
    public class OrderItemViewModel
    {
        public int Quantity { get; private set; }
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }

        public static OrderItemViewModel ToModel(OrderItem orderItem)
        {
            return new OrderItemViewModel()
            {
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId
            };
        }

        public static List<OrderItemViewModel> ToModel(List<OrderItem> orderItems)
        {
            return (orderItems.Select(orderItem => ToModel(orderItem))).ToList();
        }
    }
}

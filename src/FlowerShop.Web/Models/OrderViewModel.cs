﻿using FlowerShop.Core.Entities;
using FlowerShop.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace FlowerShop.Web.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public string Comment { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int? AddressId { get; set; }
        public int? UserId { get; set; }

        public static OrderViewModel ToModel(Order order)
        {
            return new OrderViewModel
            {
                Id = order.OrderId,
                PhoneNumber = order.PhoneNumber,
                TotalPrice = order.TotalPrice,
                Comment = order.Comment,
                OrderStatus = order.OrderStatus,
                AddressId = order.AddressId,
                UserId = order.UserId
            };
        }

        public static List<OrderViewModel> ToModel(List<Order> orders)
        {
            return (orders.Select(order => ToModel(order))).ToList();
        }
    }
}

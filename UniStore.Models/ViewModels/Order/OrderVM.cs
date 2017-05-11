namespace UniStore.Models.ViewModels.Order
{
    using System;
    using System.Collections.Generic;
    using EntityModels;
    using Enums;
    using Purchase;

    public class OrderVM
    {
        public int Id { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }

        public List<PurchaseVM> Purchases { get; set; }

        public decimal Total { get; set; }

        public string DeliveryAddress { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
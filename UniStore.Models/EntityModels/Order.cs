namespace UniStore.Models.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Enums;

    public class Order
    {
        public Order()
        {
            this.Purchases = new List<Purchase>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual List<Purchase> Purchases { get; set; }

        [Required]
        [MinLength(15, ErrorMessage = "{0} must be at least {1} symbols long!")]
        public string DeliveryAddress { get; set; }

        public OrderStatus OrderStatus { get; set; }
        
        public decimal Total
        {
            get
            {
                return this.Purchases.Sum(p => p.Value);
            }
        }
    }
}
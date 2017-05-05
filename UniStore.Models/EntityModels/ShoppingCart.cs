namespace UniStore.Models.EntityModels
{
    using System.Collections.Generic;
    using System.Linq;

    public class ShoppingCart
    {
        public ShoppingCart()
        {
            this.Purchases = new List<Purchase>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual List<Purchase> Purchases { get; set; }

        public decimal Total
        {
            get
            {
                return this.Purchases.Sum(p => p.Value);
            }
        }

        public string DeliveryAddress { get; set; }
    }
}
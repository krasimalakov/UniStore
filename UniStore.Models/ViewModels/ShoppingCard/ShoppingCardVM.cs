namespace UniStore.Models.ViewModels.ShoppingCard
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using BindingModels.Product;
    using Purchase;

    public class ShoppingCardVM
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public List<PurchaseVM> Purchases { get; set; }

        public decimal Total { get; set; }

        [DisplayName("Delivery address")]
        [Required]
        [MinLength(15, ErrorMessage = "{0} must be at least {1} symbols long!")]
        public string DeliveryAddress { get; set; }

        public SearchProductsBM SearchProductsBM { get; set; }

        public bool IsAnyPurchaseOnStock { get; set; }
    }
}
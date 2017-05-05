namespace UniStore.Models.ViewModels.Purchase
{
    using EntityModels;
    using Product;

    public class PurchaseVM
    {
        public int Id { get; set; }

        public DetailsProductVM Product { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Value { get; set; }
    }
}
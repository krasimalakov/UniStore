namespace UniStore.Models.EntityModels
{
    using System.ComponentModel.DataAnnotations;

    public class Purchase
    {
        public int Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public decimal Value => this.Quantity * this.Price;
    }
}
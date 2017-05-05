namespace UniStore.Models.EntityModels
{
    using System.Collections.Generic;

    public class SubCategory : BaseCategory
    {
        public SubCategory()
        {
            this.Products = new List<Product>();
        }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
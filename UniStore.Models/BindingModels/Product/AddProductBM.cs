namespace UniStore.Models.BindingModels.Product
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Web;

    public class AddProductBM
    {
        public int DepartmentId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int ManufacturerId { get; set; }

        public string ImageUrl { get; set; }
        public HttpPostedFileBase Image { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "{0} cannot be negative value or zero!")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "{0} cannot be negative value")]
        public int Quantity { get; set; }
    }
}